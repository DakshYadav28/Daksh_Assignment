// Handle form submission
document.getElementById('loginForm').addEventListener('submit', function (event) {
    event.preventDefault(); // Prevent the form from refreshing the page

    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    if (validateCredentials(username, password)) {
        alert('Login successful!');
        // Redirect or proceed with the application logic
    } else {
        displayError('Invalid username or password!');
    }
});

// Simulate credentials validation (replace with actual backend validation logic)
function validateCredentials(username, password) {
    const validUsername = 'admin';
    const validPassword = 'password123';

    return username === validUsername && password === validPassword;
}

// Display error message
function displayError(message) {
    const errorMessage = document.getElementById('errorMessage');
    errorMessage.textContent = message;
    errorMessage.style.display = 'block';
}