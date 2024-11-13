document.addEventListener("DOMContentLoaded", () => {
    // Анимация появления элементов при скролле
    const animatedElements = document.querySelectorAll('.animate__animated');

    const checkVisibility = () => {
        animatedElements.forEach(element => {
            const rect = element.getBoundingClientRect();
            if (rect.top < window.innerHeight && rect.bottom >= 0)
                element.classList.add('animate__fadeIn');
        });
    }

    window.addEventListener('scroll', checkVisibility);
    checkVisibility();
});