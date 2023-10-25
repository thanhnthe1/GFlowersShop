function openDrop(filter) {
    var str = "dropdown-" + filter;
    // document.getElementById('price').innerHTML=str;
    var get = document.getElementById(str);
    if (get.style.display == 'none') {
        get.style.display = 'block';
    } else {
        get.style.display = 'none';
    }
}

//current position
var pos = 0;
//number of slides
var totalSlides = $('#slider-wrap ul li').length;
//get the slide width
var sliderWidth = $('#slider-wrap').width();


$(document).ready(function () {



    //set width to be 'x' times the number of slides
    $('#slider-wrap ul#slider').width(sliderWidth * totalSlides);

    //next slide 	
    $('#next').click(function () {
        slideRight();
    });

    //previous slide
    $('#previous').click(function () {
        slideLeft();
    });




    //automatic slider
    var autoSlider = setInterval(slideRight, 3000);

    //for each slide 
    $.each($('#slider-wrap ul li'), function () {
        //set its color
        var c = $(this).attr("data-color");
        $(this).css("background", c);

        //create a pagination
        var li = document.createElement('li');
        $('#pagination-wrap ul').append(li);
    });

    //counter
    countSlides();

    //pagination
    pagination();

    //hide/show controls/btns when hover
    //pause automatic slide when hover
    $('#slider-wrap').hover(
        function () { $(this).addClass('active'); clearInterval(autoSlider); },
        function () { $(this).removeClass('active'); autoSlider = setInterval(slideRight, 3000); }
    );



});//DOCUMENT READY



function slideLeft() {
    pos--;
    if (pos == -1) { pos = totalSlides - 1; }
    $('#slider-wrap ul#slider').css('left', -(sliderWidth * pos));

    //*> optional
    countSlides();
    pagination();
}


function slideRight() {
    pos++;
    if (pos == totalSlides) { pos = 0; }
    $('#slider-wrap ul#slider').css('left', -(sliderWidth * pos));

    //*> optional 
    countSlides();
    pagination();
}




function countSlides() {
    $('#counter').html(pos + 1 + ' / ' + totalSlides);
}

function pagination() {
    $('#pagination-wrap ul li').removeClass('active');
    $('#pagination-wrap ul li:eq(' + pos + ')').addClass('active');
}



// $("#load_more_button").on("click", function () {
//     var count = document.getElementsByClassName("product-table").length;
//     $.ajax({
//         type: "post",
//         url: "/gallery",
//         data: {
//             amount: count
//         },
//         beforeSend: function () {
//             $("#load_more_button").hide();
//         },
//         success: function (data) {
//             var c = document.getElementById("load_more_products");
//             c.innerHTML += data;
//         }
//     });
// });
function closeModal() {
    document.getElementById("admin-add-modal").style.display="none";
}


