/* custom JavaScript code goes here */
$(document).ready(function(){
    $("a").filter(function(){
        return this.innerHTML.indexof("Delete")==0;
    }).click(function (){
        return confirm("Are you sure?");
    });
})