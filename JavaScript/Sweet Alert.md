Replace alert with SweetAlert
https://sweetalert.js.org/

```html
<link href="/styles/sweetalert.css" rel="stylesheet" type="text/css">   
<style type="text/css">
   .sweet-alert h2 { font-size: 21px; font-weight: 500; line-height: 23.1px; }
</style>
 
<!-- SweetAlert does not work in IE8 or prior -->
<!--[if gt IE 8 | !IE]><!-->
   <script src="/scripts/sweetalert.min.js" type="text/javascript"></script>
   <script type="text/javascript">
       window.alert = function () {
           if (arguments == null) {
               swal(" ");
           } else if (typeof arguments[0] === "number") {
               swal(arguments[0].toString());
           } else {
               swal(arguments[0] || " ");
           }
       };
   </script>
<!--<![endif]-->
```