document.querySelectorAll('.position-close-form').forEach(form => {
    form.addEventListener('submit', event => {
        if (!window.confirm('Đóng vị trí và ngừng nhận đơn mới? Các đơn đã gửi vẫn được giữ nguyên.')) {
            event.preventDefault();
        }
    });
});
