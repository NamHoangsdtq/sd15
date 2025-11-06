  'use strict';
    $(document).ready(function() {
 dashboard();

 /*Counter Js Starts*/
$('.counter').counterUp({
    delay: 10,
    time: 400
});
/*Counter Js Ends*/

//  Resource bar
    $(".resource-barchart").sparkline([5, 6, 2, 4, 9, 1, 2, 8, 3, 6, 4,2,1,5], {
              type: 'bar',
              barWidth: '15px',
              height: '80px',
              barColor: '#fff',
            tooltipClassname:'abc'
          });

            function setHeight() {
                var $window = $(window);
                var windowHeight = $(window).height();
                if ($window.width() >= 320) {
                    $('.user-list').parent().parent().css('min-height', windowHeight);
                    $('.chat-window-inner-content').css('max-height', windowHeight);
                    $('.user-list').parent().parent().css('right', -300);
                }
            };
            setHeight();

            $(window).on('load',function() {
                setHeight();
            });
        });

 $(window).resize(function() {
        dashboard();
        //  Resource bar
    $(".resource-barchart").sparkline([5, 6, 2, 4, 9, 1, 2, 8, 3, 6, 4,2,1,5], {
              type: 'bar',
              barWidth: '15px',
              height: '80px',
              barColor: '#fff',
            tooltipClassname:'abc'
          });
    });

function dashboard(){

};

 


 
