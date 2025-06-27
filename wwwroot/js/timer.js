// Temporizador global de 30 minutos para la sala de escape
(function() {
    const TIMER_KEY = 'escapeRoomTimer';
    const TOTAL_SECONDS = 45 * 60; // 45 minutos
    const timerDisplay = document.getElementById('escape-timer');
    if (!timerDisplay) {
        // Si no hay timer en la vista, no hacer nada
        return;
    }
    let intervalId;

    function getStoredTime() {
        const data = localStorage.getItem(TIMER_KEY);
        if (!data) return null;
        try {
            return JSON.parse(data);
        } catch {
            return null;
        }
    }

    function storeTime(secondsLeft) {
        localStorage.setItem(TIMER_KEY, JSON.stringify({ secondsLeft, timestamp: Date.now() }));
    }

    function clearTimer() {
        localStorage.removeItem(TIMER_KEY);
    }

    function formatTime(secs) {
        const m = Math.floor(secs / 60).toString().padStart(2, '0');
        const s = (secs % 60).toString().padStart(2, '0');
        return `${m}:${s}`;
    }

    function startTimer(secondsLeft) {
        if (!timerDisplay) return;
        timerDisplay.textContent = formatTime(secondsLeft);
        intervalId = setInterval(() => {
            secondsLeft--;
            if (secondsLeft <= 0) {
                clearInterval(intervalId);
                clearTimer();
                timerDisplay.textContent = '00:00';
                window.location.href = '/Home/Perdio';
                return;
            }
            timerDisplay.textContent = formatTime(secondsLeft);
            storeTime(secondsLeft);
        }, 1000);
    }

    // Inicialización
    let secondsLeft = TOTAL_SECONDS;
    // Si hay marca de reinicio, forzar timer en 45:00
    if (localStorage.getItem('escapeRoomTimerStart')) {
        localStorage.removeItem('escapeRoomTimerStart');
        secondsLeft = TOTAL_SECONDS;
    } else {
        const stored = getStoredTime();
        if (stored && typeof stored.secondsLeft === 'number') {
            // Calcular el tiempo real pasado
            const elapsed = Math.floor((Date.now() - stored.timestamp) / 1000);
            secondsLeft = stored.secondsLeft - elapsed;
            if (secondsLeft <= 0) {
                clearTimer();
                window.location.href = '/Home/Perdio';
                return;
            }
        }
    }
    storeTime(secondsLeft);
    startTimer(secondsLeft);

    // Opcional: limpiar el timer si el usuario gana
    window.clearEscapeTimer = clearTimer;
})(); 