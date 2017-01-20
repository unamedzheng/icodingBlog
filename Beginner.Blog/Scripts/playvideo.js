﻿
var playvideo=document.getElementById("playvideo");
playvideo.style.height=playvideo.offsetWidth*0.5+"px";

window.onload=function(){
	createVideo({id:"playvideo",autoplay:false,qqchannel:true});//单个直接调用。autoplay为false时可省略autoplay:false
	var playlist=document.getElementById("play_list").querySelectorAll("li");
	for(var i=0;i<playlist.length;i++){
		(function(i){
			playlist[i].onclick=function(){ 
				createVideo({
					id:"playvideo",
					autoplay:true,
					minWin:false,
					qqchannel:this.getAttribute("channel")=="true"?true:false,
					multiple:{
						typeid:this.getAttribute("typeid"),
						sid:this.getAttribute("sid"),
						vid:this.getAttribute("vid"),
						pic:this.getAttribute("pic")
					}
				});
			}
		})(i);
	}
}
window.onresize=function(){
	playvideo.style.height=playvideo.offsetWidth*0.5+"px";
};

