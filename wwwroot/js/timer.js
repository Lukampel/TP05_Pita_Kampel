// Temporizador global de 30 minutos para la sala de escape
(function() {
    const TIMER_KEY = 'escapeRoomTimer';
    const TOTAL_SECONDS = 30 * 60; // 30 minutos
    const timerDisplay = document.getElementById('escape-timer');
    if (!timerDisplay) {
        // Si no hay timer en la vista, limpiar cualquier timer guardado
        localStorage.removeItem(TIMER_KEY);
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
    const stored = getStoredTime();
    if (stored && typeof stored.secondsLeft === 'number') {
        // Si el valor guardado es mayor que el nuevo TOTAL_SECONDS, reiniciar el timer
        if (stored.secondsLeft > TOTAL_SECONDS) {
            secondsLeft = TOTAL_SECONDS;
            storeTime(secondsLeft);
        } else {
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