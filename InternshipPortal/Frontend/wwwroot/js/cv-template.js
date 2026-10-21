(() => {
    const builder = document.querySelector(".cv-builder");
    if (!builder) return;
    let edited = false;
    let photoUrl = null;

    function update(input) {
        const target = document.getElementById(input.dataset.cvTarget);
        if (!target) return;
        target.textContent = input.value.trim();
        if (target.hasAttribute("data-cv-optional")) target.hidden = !target.textContent;
        builder.querySelectorAll(`[data-cv-section="${input.dataset.cvTarget}"]`).forEach(section => {
            section.hidden = !target.textContent;
        });
    }
    builder.querySelectorAll("[data-cv-target]").forEach(input => {
        update(input);
        input.addEventListener("input", () => { update(input); edited = true; });
    });

    const photo = document.getElementById("cv-photo");
    const initials = document.getElementById("cv-initials");
    const photoInput = document.getElementById("cv-photo-input");
    const photoError = document.getElementById("cv-photo-error");
    function removePhoto() {
        if (photoUrl) URL.revokeObjectURL(photoUrl);
        photoUrl = null;
        photo.removeAttribute("src");
        photo.hidden = true;
        initials.hidden = false;
        photoInput.value = "";
    }
    photoInput.addEventListener("change", () => {
        const file = photoInput.files[0];
        photoError.textContent = "";
        if (!file) return;
        if (!["image/jpeg", "image/png"].includes(file.type) || file.size > 10 * 1024 * 1024) {
            photoError.textContent = "Vui lòng chọn ảnh JPG/PNG, tối đa 10 MB.";
            photoInput.value = "";
            return;
        }
        if (photoUrl) URL.revokeObjectURL(photoUrl);
        photoUrl = URL.createObjectURL(file);
        photo.onload = () => { photo.hidden = false; initials.hidden = true; edited = true; };
        photo.onerror = () => { removePhoto(); photoError.textContent = "Không đọc được ảnh. Vui lòng chọn ảnh khác."; };
        photo.src = photoUrl;
    });
    document.getElementById("cv-remove-photo").addEventListener("click", () => {
        removePhoto(); photoError.textContent = ""; edited = true;
    });
    document.getElementById("cv-print").addEventListener("click", () => window.print());
    window.addEventListener("beforeunload", event => {
        if (!edited) return;
        event.preventDefault(); event.returnValue = "";
    });
})();
