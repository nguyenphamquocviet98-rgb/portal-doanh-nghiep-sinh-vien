(() => {
    const faculty = document.getElementById("FacultyId");
    const major = document.getElementById("MajorId");
    if (!faculty || !major) return;
    const options = Array.from(major.options);
    function filterMajors() {
        for (const option of options) {
            const available = option.value === "0" || option.dataset.faculty === faculty.value;
            option.hidden = !available;
            option.disabled = !available;
        }
        const selected = major.options[major.selectedIndex];
        if (!selected || selected.disabled) major.value = "0";
    }
    faculty.addEventListener("change", filterMajors);
    filterMajors();
})();