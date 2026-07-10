gsap.registerPlugin(ScrollTrigger);

document.addEventListener("DOMContentLoaded", () => {

    const themeToggle = document.getElementById("themeToggle");
    const html = document.documentElement;
    const savedTheme = localStorage.getItem("ot-theme") || "dark";
    html.setAttribute("data-theme", savedTheme);

    if (themeToggle) {
        themeToggle.addEventListener("click", () => {
            const current = html.getAttribute("data-theme");
            const next = current === "dark" ? "light" : "dark";
            html.setAttribute("data-theme", next);
            localStorage.setItem("ot-theme", next);
        });
    }

    const mobileToggle = document.getElementById("mobileToggle");
    const navLinks = document.getElementById("navLinks");

    if (mobileToggle && navLinks) {
        mobileToggle.addEventListener("click", () => {
            mobileToggle.classList.toggle("active");
            navLinks.classList.toggle("open");
        });

        navLinks.querySelectorAll(".nav-link").forEach(link => {
            link.addEventListener("click", () => {
                mobileToggle.classList.remove("active");
                navLinks.classList.remove("open");
            });
        });
    }

    const mainNav = document.getElementById("mainNav");
    if (mainNav) {
        window.addEventListener("scroll", () => {
            if (window.scrollY > 50) {
                mainNav.classList.add("scrolled");
            } else {
                mainNav.classList.remove("scrolled");
            }
        }, { passive: true });
    }

    const sections = document.querySelectorAll("section[id]");
    const allNavLinks = document.querySelectorAll(".nav-links .nav-link");

    if (sections.length > 0 && allNavLinks.length > 0) {
        window.addEventListener("scroll", () => {
            let current = "";
            sections.forEach(section => {
                const sectionTop = section.offsetTop - 100;
                if (window.scrollY >= sectionTop) {
                    current = section.getAttribute("id");
                }
            });

            allNavLinks.forEach(link => {
                link.classList.remove("active");
                const href = link.getAttribute("href");
                if (href === "#" + current || (current === "beranda" && href === "/")) {
                    link.classList.add("active");
                }
            });
        }, { passive: true });
    }

    const heroTimeline = gsap.timeline({ defaults: { ease: "power4.out" } });

    heroTimeline
        .from(".hero-badge", {
            y: 30,
            opacity: 0,
            duration: 1
        })
        .from(".hero-title", {
            y: 60,
            opacity: 0,
            duration: 1.4
        }, "-=0.7")
        .from(".hero-subtitle", {
            y: 40,
            opacity: 0,
            duration: 1.2
        }, "-=0.9")
        .from(".hero-cta", {
            y: 30,
            opacity: 0,
            duration: 1
        }, "-=0.8")
        .from(".hero-scroll-indicator", {
            opacity: 0,
            duration: 1
        }, "-=0.5");

    document.querySelectorAll(".gsap-section").forEach(el => {
        gsap.from(el, {
            scrollTrigger: {
                trigger: el,
                start: "top 88%"
            },
            y: 50,
            opacity: 0,
            duration: 1,
            ease: "power3.out"
        });
    });

    const bentoCards = document.querySelectorAll(".gsap-bento");
    if (bentoCards.length > 0 && !window.__cardsScrollBatched) {
        window.__cardsScrollBatched = true;
        ScrollTrigger.batch(bentoCards, {
            start: "top 95%",
            onEnter: (batch) => {
                gsap.fromTo(batch,
                    { y: 30, opacity: 0 },
                    {
                        y: 0,
                        opacity: 1,
                        stagger: 0.08,
                        duration: 0.65,
                        ease: "cubic-bezier(0.16, 1, 0.3, 1)",
                        overwrite: "auto"
                    }
                );
            },
            once: true
        });
    }

    const statItems = document.querySelectorAll(".gsap-stat");
    if (statItems.length > 0) {
        ScrollTrigger.batch(statItems, {
            start: "top 95%",
            onEnter: (batch) => {
                gsap.fromTo(batch,
                    { y: 24, opacity: 0 },
                    {
                        y: 0,
                        opacity: 1,
                        stagger: 0.1,
                        duration: 0.65,
                        ease: "cubic-bezier(0.16, 1, 0.3, 1)",
                        overwrite: "auto"
                    }
                );
            },
            once: true
        });
    }

    bentoCards.forEach(card => {
        const image = card.querySelector(".gsap-parallax");
        if (!image) return;

        card.addEventListener("mousemove", (e) => {
            const rect = card.getBoundingClientRect();

            const xRatio = ((e.clientX - rect.left) / rect.width - 0.5) * 2;
            const yRatio = ((e.clientY - rect.top) / rect.height - 0.5) * 2;

            gsap.to(image, {
                x: xRatio * -10,
                y: yRatio * -10,
                rotateX: yRatio * -3,
                rotateY: xRatio * 3,
                duration: 0.4,
                ease: "cubic-bezier(0.16, 1, 0.3, 1)",
                overwrite: "auto"
            });
        });

        card.addEventListener("mouseleave", () => {
            gsap.to(image, {
                x: 0,
                y: 0,
                rotateX: 0,
                rotateY: 0,
                duration: 0.65,
                ease: "cubic-bezier(0.16, 1, 0.3, 1)",
                overwrite: "auto"
            });
        });
    });

    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener("click", function (e) {
            const targetId = this.getAttribute("href");
            if (targetId === "#") return;
            const target = document.querySelector(targetId);
            if (target) {
                e.preventDefault();
                target.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        });
    });

});
