// ========== GLOBAL LOADING OVERLAY ==========
// Create overlay element once on page load
(function () {
    const overlayDiv = document.createElement('div');
    overlayDiv.id = 'global-loading-overlay';
    overlayDiv.innerHTML = '<div class="loader-medium"></div>';
    document.body.appendChild(overlayDiv);
})();

function showGlobalLoading() {
    const overlay = document.getElementById('global-loading-overlay');
    if (overlay) overlay.classList.add('active');
}

// Automatically show overlay on any form POST submission
document.addEventListener('DOMContentLoaded', function () {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            // Only for POST methods
            if (form.method && form.method.toLowerCase() === 'post') {
                showGlobalLoading();
                // Form submits normally - overlay stays until page reloads
            }
        });
    });
});

// ========== ALERT DISMISS ==========
document.querySelectorAll('.alert-dismissible').forEach(alert => {
    const closeBtn = alert.querySelector('.btn-close');
    if (closeBtn) {
        closeBtn.addEventListener('click', () => {
            alert.style.display = 'none';
        });
    }
});

// ========== MOBILE NAVBAR TOGGLE ==========
function toggleNav() {
    const navMenu = document.getElementById('navMenu');
    if (!navMenu) return;
    if (navMenu.classList.contains('show')) {
        navMenu.classList.remove('show');
    } else {
        navMenu.classList.add('show');
    }
}