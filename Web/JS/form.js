

function tanchuang(){
    $(".jbbox").attr("style", "display:block ");
    $("#dvMask").attr("style", "display:block ");
    $("body,html").css({"overflow":"hidden"});
    $("#tanchuang2").attr("style", "display:block");
     $("body").addClass("over");
}

function tanchuang1(){
    $("#tanchuang").attr("style", "display:block ");
    $("#tanchuang2").attr("style", "display:none");
    $("#dvMask").attr("style", "display:block ");
    $("body,html").css({"overflow":"hidden"});

}

function tanchuang5(){
    $(".jbbox").attr("style", "display:none ");
    $("#tanchuang").attr("style", "display:none ");
    $("#tanchuang2").attr("style", "display:none");
    $("#dvMask").attr("style", "display:none ");
    $("body,html").attr("style","overflow:visible");

}

function tanchuang2(){
    $("#tupian_1").attr("class", "noneBtn1_1 marr6 ");
    $("#tupian_2").attr("class", "noneBtn2 marr6 ");
    $("#tupian_11").attr("style", "display:block ");
}
function tanchuang3(){
    $("#tupian_1").attr("class", "noneBtn1 marr6");
    $("#tupian_2").attr("class", "noneBtn2_1 marr6 ");
    $("#tupian_11").attr("style", "display:none ");
}
function tanchuang4(){
if( $("#tupian_3").attr("class")=="noneBtn marr6 "){
    $("#tupian_3").attr("class", "noneBtn_1 marr6 ");
    }else{
    $("#tupian_3").attr("class", "noneBtn marr6 ");
    }
}