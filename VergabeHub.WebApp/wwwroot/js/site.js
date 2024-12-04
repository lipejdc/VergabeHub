document.addEventListener('DOMContentLoaded', function () {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    var errorAlert = document.getElementById('checkboxError');
    var getNoticesButton = document.getElementById('submitButton');

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            if (this.checked) {
                // If any checkbox is checked, remove error styles from all checkboxes
                checkboxes.forEach(function (checkbox) {
                    removeErrorStyles(checkbox);
                });
                // Hide the error message
                errorAlert.style.display = 'none';
            }
        });
    });

    function removeErrorStyles(input) {
        input.classList.remove('is-invalid');
        var label = document.querySelector('label[for="' + input.id + '"]');
        label.classList.remove('is-invalid');

    }

    function addErrorStyles(input) {
        input.classList.add('is-invalid');
        var label = document.querySelector('label[for="' + input.id + '"]');
        label.classList.add('is-invalid');

    }

    getNoticesButton.addEventListener('click', function (event) {
        event.preventDefault();
        var selectedPlatforms = [];
        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedPlatforms.push(checkbox.value);
                console.log(checkbox.value)
            }
        });

        // Check if at least one checkbox is checked
        if (selectedPlatforms.length > 0) {
            // If one checkbox is selected, remove the error styles
            errorAlert.style.display = 'none';
            checkboxes.forEach(function (checkbox) {
                removeErrorStyles(checkbox);
            });

            var url = '/Index/Notices?noticePlatformCheckboxValues=' + selectedPlatforms.join(',');

            // The fetch function makes a GET request to the specified URL
            // It waits until it gets a response. However the waiting is asynchronous
            // It means JS can continue executing other tasks that are not dependent on the response
            // from the server.
            fetch(url, {
                method: 'GET'
            })
                //This specifies what should happen after the fetch operation is complete
                //When the response is received, it extracts the text content from it
                .then(response => {
                    return response.text();
                })
                //Takes the HTML content and inserts into the #table-body element
                //Also appends css styles to the table
                .then(html => {
                    console.dir($("#table-body"));
                    $("#table-body").html(html); // Use html() to replace existing content
                    // Apply custom styles to the table
                    $("head").append("<style>" +
                        "#table-body tr:nth-child(odd) { background-color: #D9E2F3; }" +
                        "#table-head { background-color: #A8D08D; }" +
                        "</style>");
                })
                .catch(error => console.error('Error:', error));
        } else {
            // Display an error message or handle the case when no checkboxes are checked
            event.preventDefault();
            errorAlert.style.display = 'block';
            checkboxes.forEach(function (checkbox) {
                addErrorStyles(checkbox);
            });
        }
    });
});
