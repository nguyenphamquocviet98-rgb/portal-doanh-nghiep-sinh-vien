document.querySelectorAll('.application-decision-form').forEach(form => {
    form.addEventListener('submit', event => {
        const target = form.querySelector('[name="TargetStatus"]').value;
        const message = target === '3'
            ? 'Duyệt ứng viên, tạo hồ sơ thực tập và tự hủy các đơn chưa kết thúc trong cùng đợt?'
            : target === '4' ? 'Từ chối đơn ứng tuyển này?' : null;
        if (message && !window.confirm(message)) event.preventDefault();
    });
});
