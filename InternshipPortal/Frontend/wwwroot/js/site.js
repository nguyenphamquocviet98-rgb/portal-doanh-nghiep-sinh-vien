// Cổng thông tin Thực tập & Kết nối Doanh nghiệp - Client JavaScript
document.addEventListener('DOMContentLoaded', function () {
    // 1. Tự động thêm hiệu ứng bóng mờ cho Navbar khi cuộn trang
    const header = document.querySelector('.site-header');
    if (header) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 20) {
                header.classList.add('shadow-sm');
                header.style.backgroundColor = 'rgba(255, 255, 255, 0.98)';
            } else {
                header.classList.remove('shadow-sm');
                header.style.backgroundColor = 'rgba(255, 255, 255, 0.95)';
            }
        });
    }

    // 2. Tự động đóng menu mobile khi nhấn vào liên kết neo (anchor link)
    const navLinks = document.querySelectorAll('.navigation a[href^="#"]');
    const navbarCollapse = document.getElementById('main-menu');
    if (navbarCollapse && typeof bootstrap !== 'undefined') {
        const bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse);
        navLinks.forEach(link => {
            link.addEventListener('click', () => {
                if (navbarCollapse.classList.contains('show')) {
                    const collapse = bsCollapse || new bootstrap.Collapse(navbarCollapse);
                    collapse.hide();
                }
            });
        });
    }

    // 3. Khởi tạo Bootstrap tooltips nếu có
    if (typeof bootstrap !== 'undefined' && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // 4. Xử lý thông báo (Notification Bell & Dropdown)
    const notifBtn = document.getElementById('notif-dropdown-btn');
    const notifBadge = document.getElementById('notif-badge');
    const notifList = document.getElementById('notif-dropdown-list');

    if (notifBtn) {
        // Tải số lượng chưa đọc khi vào trang
        fetch('/Notifications/UnreadCount')
            .then(res => res.ok ? res.json() : null)
            .then(data => {
                if (data && data.unreadCount > 0) {
                    notifBadge.textContent = data.unreadCount > 99 ? '99+' : data.unreadCount;
                    notifBadge.classList.remove('d-none');
                } else if (notifBadge) {
                    notifBadge.classList.add('d-none');
                }
            })
            .catch(() => {});

        // Khi bấm mở menu thông báo, tải 5 thông báo gần nhất
        notifBtn.parentElement.addEventListener('show.bs.dropdown', function () {
            fetch('/Notifications/Recent')
                .then(res => res.ok ? res.json() : null)
                .then(data => {
                    if (!data || !data.items || data.items.length === 0) {
                        notifList.innerHTML = '<div class="text-center text-muted small py-4">Chưa có thông báo nào</div>';
                        return;
                    }

                    let html = '';
                    data.items.forEach(item => {
                        const unreadDot = !item.isRead ? '<span class="badge bg-danger rounded-circle p-1" style="width:6px;height:6px;display:inline-block;"></span>' : '';
                        const bgStyle = !item.isRead ? 'background-color: #f0fdf4;' : '';
                        const link = item.targetUrl ? item.targetUrl : '/Notifications';

                        html += `
                            <a href="${link}" class="d-block p-2 rounded-3 text-decoration-none text-dark mb-1 transition-card border-bottom" style="${bgStyle}">
                                <div class="d-flex align-items-center justify-content-between gap-1 mb-1">
                                    <strong class="small text-truncate ${!item.isRead ? 'text-primary' : ''}">${item.title}</strong>
                                    ${unreadDot}
                                </div>
                                <div class="small text-muted text-truncate" style="font-size: 11px;">${item.message}</div>
                                <div class="text-end text-muted" style="font-size: 10px; margin-top: 2px;">${item.timeAgo}</div>
                            </a>
                        `;
                    });
                    notifList.innerHTML = html;
                })
                .catch(() => {
                    notifList.innerHTML = '<div class="text-center text-danger small py-3">Lỗi tải thông báo</div>';
                });
        });
    }
});
