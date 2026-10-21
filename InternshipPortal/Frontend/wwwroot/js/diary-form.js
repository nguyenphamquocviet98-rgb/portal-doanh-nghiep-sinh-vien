(() => {
    const form = document.getElementById("diary-form");
    if (!form) return;
    const input = document.getElementById("WeekNumber");
    const label = document.getElementById("diary-date-summary");
    const start = Date.parse(`${form.dataset.start}T00:00:00Z`);
    const end = Date.parse(`${form.dataset.end}T00:00:00Z`);
    const format = new Intl.DateTimeFormat("vi-VN", { day: "2-digit", month: "2-digit", year: "numeric", timeZone: "UTC" });
    function update() {
        const week = Number(input.value);
        if (!Number.isInteger(week) || week < 1 || week > Number(input.max)) {
            label.textContent = `Chọn tuần từ 1 đến ${input.max}.`;
            return;
        }
        const weekStart = start + (week - 1) * 7 * 86400000;
        const weekEnd = Math.min(weekStart + 6 * 86400000, end);
        label.textContent = `${format.format(new Date(weekStart))} — ${format.format(new Date(weekEnd))} · ${input.max} tuần trong kỳ`;
    }
    input.addEventListener("input", update);
    update();
})();
