/*
Template Name: Auxx - Admin & Dashboard Template
Author: Themesdesign
Version: 1.0.0
Website: https://themesdesign.in/
Contact: Themesdesign@gmail.com
File: auth Register init Js File
*/

document.getElementById('registerButton').addEventListener('click', function () {

    let isValid = true;

    // Get input values
    const username = document.getElementById('username-field').value;
    const password = document.getElementById('password').value;
    const email = document.getElementById('email-field').value;
    const mobile = document.getElementById('mobile-field').value;
    const adhar = document.getElementById('adhar-field').value;
    const roleid = document.getElementById('RoleId').value;
    const organizationid = document.getElementById('OrganizationId').value;

    // Client-side validation
    if (!username) {
        document.getElementById('username-error').textContent = 'Username is required.';
        isValid = false;
    }
    if (!password || password.length < 6) {
        document.getElementById('password-error').textContent = 'Password must be at least 6 characters.';
        isValid = false;
    }
    if (!email || !email.includes('@')) {
        document.getElementById('email-error').textContent = 'A valid email is required.';
        isValid = false;
    }

    if (!isValid) {
        return; // Stop submission if validation fails
    }
    // Submit via AJAX
    const formData = { username, password, email, mobile, adhar, roleid, organizationid };

    fetch('/Auth/Register', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById('registerMessage').textContent = 'Registration successful!';
                document.getElementById('registerMessage').className = 'text-success';

                // Optionally redirect to login page
                setTimeout(() => {
                    window.location.href = '/Auth/LoginBasic';
                }, 1500);
            } else {
                document.getElementById('registerMessage').textContent = data.message;
            }
        })
        .catch(error => {
            console.error('Error during registration:', error);
            document.getElementById('registerMessage').textContent = 'An error occurred. Please try again.';
        });
});