function hideNotification() {
    var notification = document.getElementById("notification-message");
    if (notification) { // Check if element exists
        notification.style.display = "none";
    } else {
        console.warn("Notification element not found!"); // Handle missing element (optional)
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var notification = document.getElementById("notification-message");
    if (notification) { // Check if element exists
        setTimeout(function () {
            notification.style.display = "none";
        }, 5000); // Hide after 5 seconds (adjust as needed)
    }
});
