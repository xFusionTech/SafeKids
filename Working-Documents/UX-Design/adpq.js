// JavaScript Document

$( document ).ready(function() {
	
	$('input:radio[name=getCode]').change(function() {
        if (this.value == 'email') {
            $('#textCodeForm').hide();
            $('#emailCodeForm').show();
        }
        else if (this.value == 'text') {
            $('#textCodeForm').show();
            $('#emailCodeForm').hide();
        }
    });
	
	$('#emailCode, #textCode').click(function(evt){
		evt.preventDefault();
		$('#CodeRegion').slideDown();
	});
	
	$('input:radio[name=mailingAddress]').change(function() {
        if (this.value == 'same') {
            $('#mailingAddressForm').slideUp();
        }
        else if (this.value == 'different') {
            $('#mailingAddressForm').slideDown();
        }
    });
	
	$('#loginBtn').click(function(){
		window.location.replace("app.html");
	});

	$('#textCodeForm').hide();
	$('#CodeRegion').hide();
	$('#mailingAddressForm').hide();

});



