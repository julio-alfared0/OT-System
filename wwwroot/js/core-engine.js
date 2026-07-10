
document.addEventListener('DOMContentLoaded', () => {
    document.documentElement.style.scrollBehavior = 'smooth';

    window.updateCartBadge = function() {
        var badge = document.getElementById('cart-badge');
        if (!badge) return;
        
        fetch('/Keranjang/Hitung', { cache: 'no-store' })
            .then(res => res.json())
            .then(data => {
                if (data && typeof data.total !== 'undefined') {
                    badge.textContent = data.total;
                }
            })
            .catch(() => {});
    };
    window.updateCartBadge();

    window.initPhotoSlider = function() {
        const bgElements = document.querySelectorAll('.split-image-bg');
        const imgContainers = document.querySelectorAll('.auto-photo-slider');

        const photoPool = [
            '/assets/image/iceland.webp',
            '/assets/image/blackjack.webp',
            '/assets/image/blueagave.webp',
            '/assets/image/manta-.webp',
            '/assets/image/maze-.webp'
        ];

        bgElements.forEach(bgEl => {
            let currentIndex = 0;
            setInterval(() => {
                currentIndex = (currentIndex + 1) % photoPool.length;
                const nextSrc = photoPool[currentIndex];

                if (typeof gsap !== 'undefined') {
                    gsap.to(bgEl, {
                        opacity: 0,
                        duration: 0.6,
                        ease: 'power2.inOut',
                        onComplete: () => {
                            bgEl.style.backgroundImage = `url('${nextSrc}')`;
                            gsap.to(bgEl, {
                                opacity: 1,
                                duration: 0.8,
                                ease: 'power2.out'
                            });
                        }
                    });
                } else {
                    bgEl.style.transition = 'opacity 0.6s ease';
                    bgEl.style.opacity = '0';
                    setTimeout(() => {
                        bgEl.style.backgroundImage = `url('${nextSrc}')`;
                        bgEl.style.opacity = '1';
                    }, 600);
                }
            }, 2600);
        });

        imgContainers.forEach(container => {
            let imgEl = container.querySelector('img');
            if (!imgEl) return;

            let currentIndex = 0;
            setInterval(() => {
                currentIndex = (currentIndex + 1) % photoPool.length;
                const nextSrc = photoPool[currentIndex];

                if (typeof gsap !== 'undefined') {
                    gsap.to(imgEl, {
                        opacity: 0,
                        duration: 0.6,
                        ease: 'power2.inOut',
                        onComplete: () => {
                            imgEl.src = nextSrc;
                            gsap.to(imgEl, {
                                opacity: 1,
                                duration: 0.8,
                                ease: 'power2.out'
                            });
                        }
                    });
                } else {
                    imgEl.style.transition = 'opacity 0.6s ease';
                    imgEl.style.opacity = '0';
                    setTimeout(() => {
                        imgEl.src = nextSrc;
                        imgEl.style.opacity = '1';
                    }, 600);
                }
            }, 2600);
        });
    };
    window.initPhotoSlider();

    if (typeof Swiper !== 'undefined' && document.querySelector('.hero-swiper')) {
        new Swiper('.hero-swiper', {
            effect: 'fade',
            fadeEffect: {
                crossFade: true
            },
            speed: 1200,
            autoplay: {
                delay: 3000,
                disableOnInteraction: false
            },
            loop: true,
            allowTouchMove: true
        });
    }

    if (typeof gsap !== 'undefined' && typeof ScrollTrigger !== 'undefined') {
        gsap.registerPlugin(ScrollTrigger);
        const productCards = document.querySelectorAll('.product-card, .bento-card');
        
        if (productCards.length > 0 && !window.__cardsScrollBatched) {
            window.__cardsScrollBatched = true;
            ScrollTrigger.batch(productCards, {
                start: 'top 96%',
                onEnter: (batch) => {
                    gsap.fromTo(batch,
                        { y: 24, opacity: 0 },
                        {
                            y: 0,
                            opacity: 1,
                            duration: 0.5,
                            stagger: 0.06,
                            ease: 'power3.out',
                            overwrite: 'auto'
                        }
                    );
                },
                once: true
            });
        }
    }

    if (typeof gsap !== 'undefined') {
        const interactiveCards = document.querySelectorAll('.product-card, .bento-card');

        interactiveCards.forEach((card) => {
            const productImg = card.querySelector('.product-img, .bento-img');

            card.addEventListener('mouseenter', () => {
                gsap.to(card, {
                    y: -6,
                    scale: 1.01,
                    boxShadow: '0 20px 40px rgba(0, 0, 0, 0.4), 0 0 20px rgba(212, 175, 55, 0.18)',
                    borderColor: 'rgba(212, 175, 55, 0.4)',
                    duration: 0.35,
                    ease: 'power2.out',
                    overwrite: 'auto'
                });

                if (productImg) {
                    gsap.to(productImg, {
                        scale: 1.04,
                        duration: 0.35,
                        ease: 'power2.out',
                        overwrite: 'auto'
                    });
                }
            });

            card.addEventListener('mouseleave', () => {
                gsap.to(card, {
                    y: 0,
                    scale: 1,
                    boxShadow: '0 10px 24px rgba(0, 0, 0, 0.2), inset 0 1px 1px rgba(255, 255, 255, 0.04)',
                    borderColor: 'rgba(255, 255, 255, 0.05)',
                    duration: 0.4,
                    ease: 'power2.out',
                    overwrite: 'auto'
                });

                if (productImg) {
                    gsap.to(productImg, {
                        scale: 1,
                        duration: 0.4,
                        ease: 'power2.out',
                        overwrite: 'auto'
                    });
                }
            });
        });
    }

    const cmdBadge = document.getElementById('cmd-badge');
    const commandPalette = document.getElementById('command-palette');
    const commandCard = commandPalette ? commandPalette.querySelector('.command-palette-card') : null;
    const cmdInput = document.getElementById('cmd-input');
    const openBtn = document.getElementById('open-cmd-palette');
    const closeBtn = document.getElementById('close-cmd-palette');
    const resultsContainer = document.getElementById('cmd-product-results');

    const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0 || navigator.userAgent.toUpperCase().indexOf('MAC') >= 0;
    if (cmdBadge) {
        cmdBadge.textContent = isMac ? 'Cmd + K' : 'Ctrl + K';
    }

    const openCommandPalette = () => {
        if (!commandPalette || !commandCard || typeof gsap === 'undefined') return;

        gsap.to(commandPalette, {
            opacity: 1,
            pointerEvents: 'auto',
            duration: 0.3,
            ease: 'power3.out'
        });

        gsap.fromTo(commandCard,
            { scale: 0.96, y: -15 },
            { scale: 1, y: 0, duration: 0.3, ease: 'power3.out' }
        );

        setTimeout(() => {
            if (cmdInput) cmdInput.focus();
        }, 80);
    };

    const closeCommandPalette = () => {
        if (!commandPalette || !commandCard || typeof gsap === 'undefined') return;

        gsap.to(commandPalette, {
            opacity: 0,
            pointerEvents: 'none',
            duration: 0.25,
            ease: 'power3.in'
        });

        gsap.to(commandCard, {
            scale: 0.96,
            y: -10,
            duration: 0.25,
            ease: 'power3.in'
        });

        if (cmdInput) cmdInput.blur();
    };

    window.addEventListener('keydown', (e) => {
        const isTriggerKey = (isMac && e.metaKey && e.key === 'k') || (!isMac && e.ctrlKey && e.key === 'k');
        
        if (isTriggerKey) {
            e.preventDefault();
            if (commandPalette && window.getComputedStyle(commandPalette).pointerEvents === 'auto') {
                closeCommandPalette();
            } else {
                openCommandPalette();
            }
        }

        if (e.key === 'Escape' && commandPalette && window.getComputedStyle(commandPalette).pointerEvents === 'auto') {
            closeCommandPalette();
        }
    });

    if (openBtn) openBtn.addEventListener('click', openCommandPalette);
    if (closeBtn) closeBtn.addEventListener('click', closeCommandPalette);

    if (commandPalette) {
        commandPalette.addEventListener('click', (e) => {
            if (e.target === commandPalette) {
                closeCommandPalette();
            }
        });
    }

    let searchTimeout = null;
    if (cmdInput) {
        cmdInput.addEventListener('input', (e) => {
            const query = e.target.value.toLowerCase().trim();
            const items = document.querySelectorAll('.cmd-shortcut-item');
            
            items.forEach((item) => {
                const text = item.textContent.toLowerCase();
                item.style.display = text.includes(query) ? 'flex' : 'none';
            });

            if (resultsContainer) {
                if (!query) {
                    resultsContainer.style.setProperty('display', 'none', 'important');
                    resultsContainer.innerHTML = '';
                    return;
                }

                clearTimeout(searchTimeout);
                searchTimeout = setTimeout(() => {
                    fetch('/Beranda/CariProdukJson?query=' + encodeURIComponent(query))
                        .then(res => res.json())
                        .then(products => {
                            if (products && products.length > 0) {
                                resultsContainer.style.setProperty('display', 'flex', 'important');
                                resultsContainer.innerHTML = `<span class="text-gold small fw-bold text-uppercase tracking-wider">Hasil Pencarian Produk (${products.length})</span>` + 
                                    products.map(p => `
                                        <a href="/Beranda/Detail/${p.id}" class="cmd-product-item d-flex align-items-center justify-content-between p-3 rounded-4 text-decoration-none" style="background: var(--surface-hover); border: 1px solid var(--border-subtle);">
                                            <div class="d-flex align-items-center gap-3">
                                                <div class="d-flex align-items-center justify-content-center rounded-3 bg-dark p-1" style="width: 44px; height: 44px; flex-shrink: 0;">
                                                    ${p.gambar ? `<img src="${p.gambar}" onerror="this.style.display='none'; this.nextElementSibling.style.display='block';" alt="${p.nama}" style="width: 100%; height: 100%; object-fit: contain;" /><i class="ph ph-wine fs-4 text-gold" style="display: none;"></i>` : `<i class="ph ph-wine fs-4 text-gold"></i>`}
                                                </div>
                                                <div>
                                                    <span class="fw-bold text-main d-block">${p.nama}</span>
                                                    <span class="badge bg-secondary bg-opacity-25 text-gold small font-monospace">${p.kategori}</span>
                                                </div>
                                            </div>
                                            <span class="text-gold fw-bold font-monospace small">Rp ${new Intl.NumberFormat('id-ID').format(p.harga)}</span>
                                        </a>
                                    `).join('');
                            } else {
                                resultsContainer.style.setProperty('display', 'flex', 'important');
                                resultsContainer.innerHTML = `<div class="text-muted small p-2 text-center">Produk tidak ditemukan untuk keyword "${query}"</div>`;
                            }
                        })
                        .catch(() => {});
                }, 180);
            }
        });

        cmdInput.addEventListener('keydown', (e) => {
            if (e.key === 'Enter') {
                e.preventDefault();
                const firstProduct = resultsContainer ? resultsContainer.querySelector('.cmd-product-item') : null;
                if (firstProduct) {
                    window.location.href = firstProduct.href;
                } else {
                    const firstShortcut = document.querySelector('.cmd-shortcut-item[style*="display: flex"], .cmd-shortcut-item:not([style*="display: none"])');
                    if (firstShortcut) {
                        window.location.href = firstShortcut.href;
                    } else if (cmdInput.value.trim()) {
                        window.location.href = '/Beranda/Katalog?cari=' + encodeURIComponent(cmdInput.value.trim());
                    }
                }
            }
        });
    }
});
