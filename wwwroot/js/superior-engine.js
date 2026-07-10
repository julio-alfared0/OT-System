
document.addEventListener('DOMContentLoaded', () => {
    if (typeof Lenis !== 'undefined' && !window.__lenisInstance) {
        const lenis = new Lenis({
            duration: 1.1,
            easing: (t) => Math.min(1, 1.001 - Math.pow(2, -10 * t)),
            direction: 'vertical',
            gestureDirection: 'vertical',
            smooth: true,
            smoothTouch: false,
            wheelMultiplier: 1.0,
            touchMultiplier: 1.5
        });

        window.__lenisInstance = lenis;

        if (typeof gsap !== 'undefined' && typeof ScrollTrigger !== 'undefined') {
            gsap.registerPlugin(ScrollTrigger);
            lenis.on('scroll', ScrollTrigger.update);
            gsap.ticker.add((time) => {
                lenis.raf(time * 1000);
            });
            gsap.ticker.lagSmoothing(0);
        } else {
            function raf(time) {
                lenis.raf(time);
                requestAnimationFrame(raf);
            }
            requestAnimationFrame(raf);
        }
    }

    const cmdBadge = document.getElementById('cmd-badge');
    const commandPalette = document.getElementById('command-palette');
    const commandCard = commandPalette ? commandPalette.querySelector('.command-palette-card') : null;
    const cmdInput = document.getElementById('cmd-input');
    const openBtn = document.getElementById('open-cmd-palette');
    const closeBtn = document.getElementById('close-cmd-palette');

    const isMac = navigator.platform.toUpperCase().indexOf('MAC') >= 0 || navigator.userAgent.toUpperCase().indexOf('MAC') >= 0;
    if (cmdBadge) {
        cmdBadge.textContent = isMac ? 'Cmd + K' : 'Ctrl + K';
    }

    const openCommandPalette = () => {
        if (!commandPalette || !commandCard || typeof gsap === 'undefined') return;

        gsap.to(commandPalette, {
            opacity: 1,
            pointerEvents: 'auto',
            duration: 0.4,
            ease: 'expo.out'
        });

        gsap.fromTo(commandCard,
            { scale: 0.95, y: -20 },
            { scale: 1, y: 0, duration: 0.4, ease: 'expo.out' }
        );

        setTimeout(() => {
            if (cmdInput) cmdInput.focus();
        }, 100);
    };

    const closeCommandPalette = () => {
        if (!commandPalette || !commandCard || typeof gsap === 'undefined') return;

        gsap.to(commandPalette, {
            opacity: 0,
            pointerEvents: 'none',
            duration: 0.3,
            ease: 'expo.out'
        });

        gsap.to(commandCard, {
            scale: 0.95,
            y: -10,
            duration: 0.3,
            ease: 'expo.out'
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

    if (cmdInput) {
        cmdInput.addEventListener('input', (e) => {
            const query = e.target.value.toLowerCase().trim();
            const items = document.querySelectorAll('.cmd-shortcut-item');
            items.forEach((item) => {
                const text = item.textContent.toLowerCase();
                if (text.includes(query)) {
                    item.style.display = 'flex';
                } else {
                    item.style.display = 'none';
                }
            });
        });
    }

    const cards = document.querySelectorAll('.product-card, .bento-card');

    cards.forEach((card) => {
        const img = card.querySelector('.bento-img, img');

        card.addEventListener('mouseenter', () => {
            if (typeof gsap === 'undefined') return;

            gsap.to(card, {
                y: -8,
                scale: 1.015,
                boxShadow: '0 32px 64px rgba(0, 0, 0, 0.65), 0 0 24px rgba(212, 175, 55, 0.22)',
                borderColor: 'rgba(212, 175, 55, 0.35)',
                duration: 0.55,
                ease: 'cubic-bezier(0.16, 1, 0.3, 1)',
                overwrite: 'auto'
            });

            if (img) {
                gsap.to(img, {
                    scale: 1.05,
                    y: -6,
                    duration: 0.55,
                    ease: 'cubic-bezier(0.16, 1, 0.3, 1)',
                    overwrite: 'auto'
                });
            }
        });

        card.addEventListener('mouseleave', () => {
            if (typeof gsap === 'undefined') return;

            gsap.to(card, {
                y: 0,
                scale: 1,
                boxShadow: '0 24px 48px rgba(0, 0, 0, 0.4), inset 0 1px 1px rgba(255, 255, 255, 0.08)',
                borderColor: 'rgba(255, 255, 255, 0.04)',
                duration: 0.6,
                ease: 'cubic-bezier(0.16, 1, 0.3, 1)',
                overwrite: 'auto'
            });

            if (img) {
                gsap.to(img, {
                    scale: 1,
                    y: 0,
                    duration: 0.6,
                    ease: 'cubic-bezier(0.16, 1, 0.3, 1)',
                    overwrite: 'auto'
                });
            }
        });

        card.addEventListener('touchstart', () => {
            if (typeof gsap === 'undefined') return;

            gsap.to(card, {
                scale: 0.985,
                duration: 0.25,
                ease: 'expo.out',
                overwrite: 'auto'
            });
        }, { passive: true });

        card.addEventListener('touchend', () => {
            if (typeof gsap === 'undefined') return;

            gsap.to(card, {
                scale: 1,
                duration: 0.35,
                ease: 'expo.out',
                overwrite: 'auto'
            });
        }, { passive: true });
    });

    if (typeof gsap !== 'undefined' && typeof ScrollTrigger !== 'undefined') {
        const revealElements = document.querySelectorAll('.scroll-reveal');
        
        if (revealElements.length > 0 && !window.__revealBatched) {
            window.__revealBatched = true;
            ScrollTrigger.batch(revealElements, {
                start: 'top 95%',
                onEnter: (elements) => {
                    gsap.fromTo(elements, 
                        { opacity: 0, y: 28 },
                        {
                            opacity: 1,
                            y: 0,
                            stagger: 0.1,
                            duration: 0.65,
                            ease: 'cubic-bezier(0.16, 1, 0.3, 1)',
                            overwrite: 'auto'
                        }
                    );
                },
                once: true
            });
        }
    }
});
