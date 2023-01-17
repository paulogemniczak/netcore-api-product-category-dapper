window.addEventListener('DOMContentLoaded', (event) => {
  setTimeout(
    function() {
      $(".title:first").html(
      `<h2 class="title">
         Gemniczak.API
         <span>
           <small>
             <pre class="version">1.0</pre>
           </small>
           <small class="version-stamp">
             <pre class="version">OAS3</pre>
           </small>
         </span>
       </h2>`);
    }, 1000);
});
