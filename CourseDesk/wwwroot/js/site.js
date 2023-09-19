// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Add a change event listener to the radio button with the id "card"
    $("#card").change(function () {
        if ($(this).is(":checked")) {
            $("#card_div").show(); // Show the card_div
            $("#upi_div").hide();
        }
        else {
            $("#card_div").hide(); // Hide the card_div
        }
    });

    $("#upi").change(function () {
        if ($(this).is(":checked")) {
            $("#upi_div").show();
            $("#card_div").hide();
        }
        else {
            $("#upi_div").hide();
        }
    });

    $("#proceedButton").click(function () {
        // Check which payment method is selected
        var selectedPaymentMethod = $("input[name='payment']:checked").attr("id");
        var message_span = $("#error");
        var cartid = @Model.Id;
        console.log("cartid id " + cartid);
        // Collect form values based on the selected payment method
        var formData = {};
        if (selectedPaymentMethod === "card") {
            formData.Name = $("#Name").val();
            formData.Number = $("#Number").val();
            formData.cvv = $("#cvv").val();
        }
        else if (selectedPaymentMethod === "upi") {
            formData.payment_address = $("#payment_address").val();
        }
        else {
            formData = null;
        }

        $.ajax({
            type: "POST",
            url: "/Payment/Proceed", // Replace with your actual controller and action
            data: { form: formData, mode: selectedPaymentMethod },
            success: function (response) {
                // Handle the response from the controller action
                console.log("Response:", response);
                if (response.success == true) {
                    message_span.text("Successful");
                    window.location.href = "/Payment/Success/" + cartid;
                }
                else {
                    
                    message_span.text(response.message);
                }
                // You can redirect, display a message, or perform other actions here
            },
            error: function (xhr, status, error) {
                // Handle any errors that occur during the AJAX request
                console.error("Error:", error);
            }
        });

        // You can now use the formData object to do something with the collected data
        console.log(formData);

        // Optionally, you can submit the form here or perform any other action.
    });
});