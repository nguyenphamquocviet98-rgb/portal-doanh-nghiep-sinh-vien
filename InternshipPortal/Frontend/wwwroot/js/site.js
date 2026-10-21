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
                    notifBtn.classList.add('has-unread-bell');
                } else if (notifBadge) {
                    notifBadge.classList.add('d-none');
                    notifBtn.classList.remove('has-unread-bell');
                }
            })
            .catch(() => {});

        // Khi bấm mở menu thông báo, tải 5 thông báo gần nhất
        notifBtn.parentElement.addEventListener('show.bs.dropdown', function () {
            notifBtn.classList.remove('has-unread-bell');
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

    // 5. Scroll Reveal Animations (IntersectionObserver)
    if ('IntersectionObserver' in window) {
        // Tự động gán lớp animate-on-scroll cho các thành phần chính
        const animTargets = document.querySelectorAll(
            '.portal-v24h-hero, #quy-trinh .col-md-3, section.mb-5, .card.transition-card, .table-card'
        );

        animTargets.forEach((el, index) => {
            if (!el.classList.contains('animate-on-scroll')) {
                el.classList.add('animate-on-scroll');
                // Gán độ trễ bậc thang nếu là các thẻ trong cùng một lưới
                if (el.parentElement && el.parentElement.classList.contains('row')) {
                    const delayClass = `delay-${(index % 4) + 1}`;
                    el.classList.add(delayClass);
                }
            }
        });

        const revealObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('is-visible');
                    observer.unobserve(entry.target);
                }
            });
        }, {
            threshold: 0.1,
            rootMargin: '0px 0px -40px 0px'
        });

        document.querySelectorAll('.animate-on-scroll').forEach(el => {
            revealObserver.observe(el);
        });
    } else {
        // Dự phòng cho trình duyệt cũ: hiển thị trực tiếp
        document.querySelectorAll('.animate-on-scroll').forEach(el => el.classList.add('is-visible'));
    }

    // 6. Hiệu ứng Micro-feedback cho các nút bấm chính
    document.querySelectorAll('.btn-success, .account-submit').forEach(btn => {
        btn.addEventListener('click', function (e) {
            const rect = this.getBoundingClientRect();
            const ripple = document.createElement('span');
            ripple.style.position = 'absolute';
            ripple.style.borderRadius = '50%';
            ripple.style.transform = 'scale(0)';
            ripple.style.backgroundColor = 'rgba(255, 255, 255, 0.4)';
            ripple.style.pointerEvents = 'none';

            const size = Math.max(rect.width, rect.height);
            ripple.style.width = ripple.style.height = `${size}px`;
            ripple.style.left = `${e.clientX - rect.left - size / 2}px`;
            ripple.style.top = `${e.clientY - rect.top - size / 2}px`;

            ripple.animate([
                { transform: 'scale(0)', opacity: 1 },
                { transform: 'scale(2.5)', opacity: 0 }
            ], {
                duration: 500,
                easing: 'ease-out'
            }).onfinish = () => ripple.remove();

            this.appendChild(ripple);
        });
    });
});
