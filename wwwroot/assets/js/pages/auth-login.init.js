/*
Template Name: Auxx - Admin & Dashboard Template
Author: Themesdesign
Version: 1.0.0
Website: https://themesdesign.in/
Contact: Themesdesign@gmail.com
File: auth login init Js File
*/

document.getElementById('signInForm').addEventListener('submit', function (event) {
   event.preventDefault(); // Prevent the form from submitting

    // Get input values
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    // Reset error messages
    document.getElementById('username-error').textContent = '';
    document.getElementById('password-error').textContent = '';
    document.getElementById('successAlert').textContent = '';

    // Client-side validation
    let isValid = true;

    if (!username) {
        document.getElementById('username-error').textContent = 'Username is required';
        isValid = false;
    }

    if (!password) {
        document.getElementById('password-error').textContent = 'Password is required';
        isValid = false;
    }

    if (!isValid) {
        return; // Stop if validation fails
    }

    // Send data via AJAX
    const formData = { username, password };


    fetch('/Auth/LogIn', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
        credentials: 'include', // Ensures cookies are sent with requests
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById('successAlert').textContent = 'Signin successful!';
                document.getElementById('successAlert').className = 'text-success';

                // Redirect to dashboard after successful login
                setTimeout(() => {
                    window.location.href = '/Dashboard/Index';
                }, 1000);
            } else {
                document.getElementById('successAlert').textContent = data.message;
                document.getElementById('successAlert').className = 'text-danger';
            }
        })
        .catch(error => {
            console.error('Error during signin:', error);
            document.getElementById('successAlert').textContent = 'An error occurred. Please try again.';
        });
    //// Define regular expressions for validation
    //const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    //const strongPasswordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;

    //// Validate username/email and password
    //const usernameError = document.getElementById('username-error');
    //const passwordError = document.getElementById('password-error');
    //const successAlert = document.getElementById('successAlert');
    //const rememberMeCheckbox = document.getElementById('checkboxDefault1');
    //const rememberError = document.getElementById('remember-error');

    //usernameError.classList.add('hidden'); // Hide any previous error message
    //passwordError.classList.add('hidden');
    //successAlert.classList.add('hidden'); // Hide the success message

    //if (!emailRegex.test(username)) {
    //    usernameError.classList.remove('hidden'); // Show error message
    //} else if (!strongPasswordRegex.test(password)) {
    //    passwordError.classList.remove('hidden'); // Show error message
    //} else {
    //    // Form is valid, show the success message
    //    successAlert.classList.remove('hidden');
    //}

    //if (!rememberMeCheckbox.checked) {
    //    // Prevent the form from submitting if the checkbox is not checked
    //    event.preventDefault();
    //    rememberError.classList.remove('hidden');
    //} else {
    //    rememberError.classList.add('hidden');
    //}

});