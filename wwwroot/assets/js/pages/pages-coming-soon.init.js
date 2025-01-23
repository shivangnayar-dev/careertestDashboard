/*
Template Name: Auxx - Admin & Dashboard Template
Author: Themesdesign
Website: https://themesdesign.in/
Contact: Themesdesign@gmail.com
File: pages coming soon init js

*/document.addEventListener('DOMContentLoaded', function () {
    var mainArray = Array.from(document.querySelectorAll(".countdownlist"))
    mainArray.forEach(function (item) {
        var countdownVal = new Date().getTime() + 12000; // item.getAttribute("data-countdown") // You may use specific date for long duration of coming soon time.

        // Set the date we're counting down to
        var countDownDate = new Date(countdownVal).getTime();

        // Update the count down every 1 second
        var countDown = setInterval(function () {
            // Get today's date and time
            var currentTime = new Date().getTime();
            // Find the distance between currentTime and the count down date
            var distance = countDownDate - currentTime;

            // Time calculations for days, hours, minutes and seconds
            var days = Math.floor(distance / (1000 * 60 * 60 * 24));
            var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

            var countDownBlock = '<div class="flex flex-col items-center justify-center w-24 h-24 p-2 text-center bg-white/10 rounded countdownlist-item">' +
                '<div class="count-title text-white/75">Days</div>' + '<div class="mt-2 text-2xl font-semibold count-num">' + days + '</div>' +
                '</div>' +
                '<div class="flex flex-col items-center justify-center w-24 h-24 p-2 text-center bg-white/10 rounded countdownlist-item">' +
                '<div class="count-title text-white/75">Hours</div>' + '<div class="mt-2 text-2xl font-semibold count-num">' + hours + '</div>' +
                '</div>' +
                '<div class="flex flex-col items-center justify-center w-24 h-24 p-2 text-center bg-white/10 rounded countdownlist-item">' +
                '<div class="count-title text-white/75">Minutes</div>' + '<div class="mt-2 text-2xl font-semibold count-num">' + minutes + '</div>' +
                '</div>' +
                '<div class="flex flex-col items-center justify-center w-24 h-24 p-2 text-center bg-white/10 rounded countdownlist-item">' +
                '<div class="count-title text-white/75">Seconds</div>' + '<div class="mt-2 text-2xl font-semibold count-num">' + seconds + '</div>' +
                '</div>';

            // Output the result in an element with id="countDownBlock"
            if (item) {
                item.innerHTML = countDownBlock;
            }
            // If the count down is over, write some text 
            if (distance < 0) {
                clearInterval(countDown);
                document.getElementById("countDownText").innerHTML = `<div class="text-center">
                <i class="text-4xl text-green-500 ri-verified-badge-line"></i>
                <div class="mt-5 mb-4">
                    <h4 class="mb-2 text-white">We've Launched our new website</h4>
                    <p class="text-white/75">Click the below button to visit our website.</p>
                </div>
                <a href="/" class="text-white btn bg-custom-500 border-custom-500 hover:text-white hover:bg-custom-600 hover:border-custom-600 focus:text-white focus:bg-custom-600 focus:border-custom-600 active:text-white active:bg-custom-600 active:border-custom-600">Back to Home</a>
            </div>`;
            }
        }, 1000);
    })
});