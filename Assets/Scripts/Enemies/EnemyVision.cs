using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private EnemyData _data;

    public event Action PlayerSpotted; // Игрок в поле зрения
    public event Action PlayerLost;    // Игрок потерян (не виден, не слышим, время вышло)
    public event Action PlayerAlerted; // Игрок в ближней зоне, но не видим (только если был замечен ранее)

    private bool _isPlayerVisible = false;
    private bool _wasPlayerSpotted = false; // Флаг: видел ли враг игрока хотя бы раз?
    private bool _isPlayerAlerted = false;  // Флаг: "слышит" ли враг игрока в ближней зоне?
    private float _lastSightingTime = Mathf.NegativeInfinity; // Время последнего видения
    // private float _lastAlertTime = Mathf.NegativeInfinity; // Не обязательно, если используем _lastSightingTime для потери

    public EnemyData Data => _data;
    private void Start()
    {
        StartCoroutine(VisionCheckRoutine());
    }

    private System.Collections.IEnumerator VisionCheckRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_data.ViewDelay);

        while (true)
        {
            bool currentlyInField = _fieldOfView.IsPlayerInField();

            // --- Обработка основного поля зрения ---
            if (currentlyInField)
            {
                if (!_isPlayerVisible)
                {
                    // Игрок только что замечен!
                    _isPlayerVisible = true;
                    _wasPlayerSpotted = true; // Отмечаем, что враг его видел
                    _lastSightingTime = Time.time;
                    PlayerSpotted?.Invoke();
                    // Если был в состоянии "тревоги", сбрасываем его
                    if (_isPlayerAlerted)
                    {
                        _isPlayerAlerted = false;
                        // Не отправляем отдельное событие сброса тревоги, логика Follow/LookAround это учтёт
                    }
                }
                else
                {
                    // Уже видим, обновляем время
                    _lastSightingTime = Time.time;
                }
            }
            else // Игрок НЕ в поле зрения
            {
                if (_isPlayerVisible)
                {
                    _isPlayerVisible = false;
                }
            }

            // --- Обработка ближней зоны ---
            // ВАЖНО: Проверяем ближнюю зону ТОЛЬКО если враг уже видел игрока (_wasPlayerSpotted)
            if (_wasPlayerSpotted && !_isPlayerVisible) // Не видим, но уже замечал
            {
                bool currentlyInNearbyZone = _fieldOfView.IsPlayerInNearestZone();

                if (currentlyInNearbyZone)
                {
                    if (!_isPlayerAlerted)
                    {
                        // Игрок "слышим" в ближней зоне!
                        _isPlayerAlerted = true;
                        // _lastSightingTime не обновляем, т.к. это не "видение", но мы знаем, что он рядом
                        // Можно обновить до текущего времени, чтобы отсчёт потери шёл от "последнего контакта"
                        _lastSightingTime = Time.time;
                        PlayerAlerted?.Invoke(); // Отправляем сигнал тревоги
                    }
                    else
                    {
                         // Уже "слышим", просто обновляем время последнего "контакта"
                         _lastSightingTime = Time.time;
                    }
                }
                else // Не в ближней зоне
                {
                    if (_isPlayerAlerted)
                    {
                        _isPlayerAlerted = false;
                    }
                }
            }
            else // !_wasPlayerSpotted ИЛИ _isPlayerVisible
            {
                 // Если враг НИКОГДА не видел игрока или сейчас его видит, ближняя зона неактивна.
                 // Сбрасываем флаг "тревоги", если он был (например, враг только что увидел игрока, тревога сбрасывается).
                 if (_isPlayerAlerted)
                 {
                     _isPlayerAlerted = false;
                 }
            }

            // --- Проверка: пора "считать, что потерял"? ---
            // Потеря происходит, если:
            // 1. Игрок не видим (_isPlayerVisible == false)
            // 2. Игрок не "слышим" в ближней зоне (_isPlayerAlerted == false)
            // 3. Прошло достаточно времени с последнего "контакта" (_lastSightingTime)
            if (!_isPlayerVisible && !_isPlayerAlerted && (Time.time - _lastSightingTime) >= _data.AlertTime)
            {
                 if (_wasPlayerSpotted) // Проверяем, был ли враг в состоянии "знаю, что он был"?
                 {
                     // Считаем, что игрок потерян
                     PlayerLost?.Invoke();
                     // Сбрасываем флаг, если хочется, чтобы после потери снова нужно было "увидеть" для включения тревоги
                     // _wasPlayerSpotted = false; // <- Опционально. Зависит от желаемого поведения.
                     // Если оставить _wasPlayerSpotted = true, враг сразу "услышит" при вхождении в ближнюю зону.
                     // Если сбросить, игрок может снова "обойти" сзади, пока враг не увидит его снова.
                     // Скорее всего, сброс *не нужен*, чтобы тревога оставалась активной после одного замечания.
                 }
            }

            yield return delay;
        }
    }
}