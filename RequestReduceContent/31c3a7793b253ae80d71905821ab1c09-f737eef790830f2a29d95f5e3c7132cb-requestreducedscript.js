(function(n){n.fn.tipsy=function(t){return t=n.extend({},n.fn.tipsy.defaults,t),this.each(function(){var i=n.fn.tipsy.elementOptions(this,t);n(this).hover(function(){var t,e,r;n.data(this,"cancel.tipsy",!0),t=n.data(this,"active.tipsy"),t||(t=n('<div class="tipsy"><div class="tipsy-inner"/></div>'),t.css({position:"absolute",zIndex:1e5}),n.data(this,"active.tipsy",t)),(n(this).attr("title")||typeof n(this).attr("original-title")!="string")&&n(this).attr("original-title",n(this).attr("title")||"").removeAttr("title"),typeof i.title=="string"?e=n(this).attr(i.title=="title"?"original-title":i.title):typeof i.title=="function"&&(e=i.title.call(this)),t.find(".tipsy-inner")[i.html?"html":"text"](e||i.fallback),r=n.extend({},n(this).offset(),{width:this.offsetWidth,height:this.offsetHeight}),t.get(0).className="tipsy",t.remove().css({top:0,left:0,visibility:"hidden",display:"block"}).appendTo(document.body);var f=t[0].offsetWidth,u=t[0].offsetHeight,o=typeof i.gravity=="function"?i.gravity.call(this):i.gravity;switch(o.charAt(0)){case"n":t.css({top:r.top+r.height,left:r.left+r.width/2-f/2}).addClass("tipsy-north");break;case"s":t.css({top:r.top-u,left:r.left+r.width/2-f/2}).addClass("tipsy-south");break;case"e":t.css({top:r.top+r.height/2-u/2,left:r.left-f}).addClass("tipsy-east");break;case"w":t.css({top:r.top+r.height/2-u/2,left:r.left+r.width}).addClass("tipsy-west")}i.fade?t.css({opacity:0,display:"block",visibility:"visible"}).animate({opacity:.8}):t.css({visibility:"visible"})},function(){n.data(this,"cancel.tipsy",!1);var t=this;setTimeout(function(){if(n.data(this,"cancel.tipsy"))return;var r=n.data(t,"active.tipsy");i.fade?r.stop().fadeOut(function(){n(this).remove()}):r.remove()},100)})})},n.fn.tipsy.elementOptions=function(t,i){return n.metadata?n.extend({},i,n(t).metadata()):i},n.fn.tipsy.defaults={fade:!1,fallback:"",gravity:"n",html:!1,title:"title"},n.fn.tipsy.autoNS=function(){return n(this).offset().top>n(document).scrollTop()+n(window).height()/2?"s":"n"},n.fn.tipsy.autoWE=function(){return n(this).offset().left>n(document).scrollLeft()+n(window).width()/2?"e":"w"}})(jQuery)