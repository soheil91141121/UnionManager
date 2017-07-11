var Script = function () {

    $.validator.setDefaults({
        submitHandler: function() { alert("submitted!"); }
    });

    $().ready(function () {
        // validate signup form on keyup and submit
        $("#frm_AddTrade").validate({
            rules: {
                txt_Name: "required",
                cb_Group: "",
                txt_Tel: "required",
                txt_Address: "required"
            },
            messages: {
                txt_Name: "گروه صنف را وارد کنید",
                cb_Group: "Please enter your cb",
                txt_Tel: "شماره تلفن را وارد کنید",
                txt_Address: "آدرس را وارد کنید"
            }
        });

        // propose username by combining first- and lastname
        $("#username").focus(function() {
            var firstname = $("#firstname").val();
            var lastname = $("#lastname").val();
            if(firstname && lastname && !this.value) {
                this.value = firstname + "." + lastname;
            }
        });

        //code to hide topic selection, disable for demo
        var newsletter = $("#newsletter");
        // newsletter topics are optional, hide at first
        var inital = newsletter.is(":checked");
        var topics = $("#newsletter_topics")[inital ? "removeClass" : "addClass"]("gray");
        var topicInputs = topics.find("input").attr("disabled", !inital);
        // show when newsletter is checked
        newsletter.click(function() {
            topics[this.checked ? "removeClass" : "addClass"]("gray");
            topicInputs.attr("disabled", !this.checked);
        });
    });


}();