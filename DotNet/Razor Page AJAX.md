# Method 1: Manually Submit
```html
@page
@model Dynamic1Model

<div>
    <label>First</label>
    <input id=FirstName />
</div>
<div>
    <label>Last</label>
    <input id=LastName />
</div>
<div>
    <input id=save-person type=button value=Submit>
</div>
<hr>
<div id=output>Output goes here</div>

<form id=post-people-form asp-page="People" method=post></form>

<script>

    // POST AJAX by pulling token from empty form
    jQuery(function() {
        jQuery("#save-person").click(function() {
            jQuery("#output").html("working...");
            var data = {
                FirstName: jQuery("#FirstName").val(),
                LastName: jQuery("#LastName").val()
            };
            // Retrieve the CSRF token from the form
            jQuery("#post-people-form :input").each(function() {
                data[jQuery(this).attr("name")] = jQuery(this).val();
            });
            jQuery.ajax({
                type: "post",
                url: "People",
                data: data,
                dataType: "html"
            }).done(function(result) {
                jQuery("#output").html(result);
            });
        });
    });


    // GET AJAX call on page load
    jQuery(function() {
        jQuery("#output").html("working...");
        jQuery.ajax({
            type: "get",
            url: "People",
            dataType: "html"
        }).done(function(result) {
            jQuery("#output").html(result);
        });
    });

</script>
```

# Method 2: Serialize

```html
@page
@model Dynamic1Model

<div>
    <label>First</label>
    <input id=FirstName />
</div>
<div>
    <label>Last</label>
    <input id=LastName />
</div>
<div>
    <input id=save-person type=button value=Submit>
</div>
<hr>
<div id=output>Output goes here</div>

<form id=post-people-form asp-page="People" method=post></form>

<script>

    // POST AJAX by populating a form
    jQuery(function() {
        jQuery("#save-person").click(function() {
            jQuery("#output").html("working...");
            var $form = jQuery("#post-people-form");
            $form.find(":input").each(function() {
                if (jQuery(this).attr("name") !== "__RequestVerificationToken") {
                    jQuery(this).remove();
                }
            });
            $form.append(jQuery("<input type=hidden name=FirstName>")
                .val(jQuery("#FirstName").val()));
            $form.append(jQuery("<input type=hidden name=LastName>")
                .val(jQuery("#LastName").val()));

            jQuery.ajax({
                type: $form.attr("method"),
                url: $form.attr("action"),
                data: $form.serialize(),
                dataType: "html"
            }).done(function(result) {
                jQuery("#output").html(result);
            });
        });
    });


    // GET AJAX call on page load
    jQuery(function() {
        jQuery("#output").html("working...");
        jQuery.ajax({
            type: "get",
            url: "People",
            dataType: "html"
        }).done(function(result) {
            jQuery("#output").html(result);
        });
    });

</script>
```

# Method 3: Actual Form
```html
@page
@model IndexModel
@{
    ViewData["Title"] = "Home page";
}

<form asp-page="People" method=post>
    <div>
        <label>First</label>
        <input name=FirstName />
    </div>
    <div>
        <label>Last</label>
        <input name=LastName />
    </div>
    <div>
        <input id=save-person type=button value=Submit>
    </div>
</form>
<hr>
<div id=output>Output goes here</div>

<script>

    // GET AJAX call on page load
    jQuery(function() {
        jQuery("#output").html("working...");
        jQuery.ajax({
            type: "get",
            url: "People",
            dataType: "html"
        }).done(function(result) {
            jQuery("#output").html(result);
        });
    });

    // POST AJAX call when you click the button
    jQuery("#save-person").click(function() {
        jQuery("#output").html("working...");
        var $form = jQuery(this).closest("form");
        jQuery.ajax({
            type: $form.attr("method"),
            url: $form.attr("action"),
            data: $form.serialize(),
            dataType: "html"
        }).done(function(result) {
            jQuery("#output").html(result);
        });
    });
</script>
```