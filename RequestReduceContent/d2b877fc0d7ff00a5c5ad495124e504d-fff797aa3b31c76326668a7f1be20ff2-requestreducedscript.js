(function(n,t){function u(t,i){return t=t?' id="'+s+t+'"':"",i=i?' style="'+i+'"':"",n("<div"+t+i+"/>")}function a(n,t){return t=t==="x"?c.width():c.height(),typeof n=="string"?Math.round(/%/.test(n)?t/100*parseInt(n,10):parseInt(n,10)):n}function pt(n){return i.photo||/\.(gif|png|jpg|jpeg|bmp)(?:\?([^#]*))?(?:#(\.*))?$/i.test(n)}function ei(t){for(var i in t)n.isFunction(t[i])&&i.substring(0,2)!=="on"&&(t[i]=t[i].call(l));return t.rel=t.rel||l.rel||"nofollow",t.href=t.href||n(l).attr("href"),t.title=t.title||l.title,t}function d(t,i){i&&i.call(l),n.event.trigger(t)}function li(){var t,n=s+"Slideshow_",u="click."+s,c,h,l;i.slideshow&&e[1]&&(c=function(){it.text(i.slideshowStop).unbind(u).bind(bt,function(){(f<e.length-1||i.loop)&&(t=setTimeout(r.next,i.slideshowSpeed))}).bind(wt,function(){clearTimeout(t)}).one(u,h);o.removeClass(n+"off").addClass(n+"on"),t=setTimeout(r.next,i.slideshowSpeed)},h=function(){clearTimeout(t);it.text(i.slideshowStart).unbind(bt+" "+wt+" "+u).one(u,c);o.removeClass(n+"on").addClass(n+"off")},it.bind(oi,function(){clearTimeout(t)}),o.hasClass(n+"on")||i.slideshowAuto&&!o.hasClass(n+"off")?c():h())}function fi(t){if(!lt){l=t,i=ei(n.extend({},n.data(l,y))),e=n(l),f=0,i.rel!=="nofollow"&&(e=n("."+ot).filter(function(){var t=n.data(this,y).rel||this.rel;return t===i.rel}),f=e.index(l),f===-1&&(e=e.add(l),f=e.length-1));if(!p){p=rt=!0,o.show(),vt=l;try{vt.blur()}catch(u){}k.css({opacity:+i.opacity,cursor:i.overlayClose?"pointer":"auto"}).show(),i.w=a(i.initialWidth,"x"),i.h=a(i.initialHeight,"y"),r.position(0),yt&&c.bind("resize."+at+" scroll."+at,function(){k.css({width:c.width(),height:c.height(),top:c.scrollTop(),left:c.scrollLeft()})}).trigger("scroll."+at),d(hi,i.onOpen),ti.add(ft).add(et).add(it).add(dt).hide(),gt.html(i.close).show()}r.load(!0)}}var ui={transition:"elastic",speed:300,width:!1,initialWidth:"600",innerWidth:!1,maxWidth:!1,height:!1,initialHeight:"450",innerHeight:!1,maxHeight:!1,scalePhotos:!0,scrolling:!0,inline:!1,html:!1,iframe:!1,photo:!1,href:!1,title:!1,rel:!1,opacity:.9,preloading:!0,current:"image {current} of {total}",previous:"previous",next:"next",close:"close",open:!1,loop:!0,slideshow:!1,slideshowAuto:!0,slideshowSpeed:2500,slideshowStart:"start slideshow",slideshowStop:"stop slideshow",onOpen:!1,onLoad:!1,onComplete:!1,onCleanup:!1,onClosed:!1,overlayClose:!0,escKey:!0,arrowKey:!0},y="colorbox",s="cbox",hi=s+"_open",wt=s+"_load",bt=s+"_complete",ci=s+"_cleanup",oi=s+"_closed",ct=s+"_purge",si=s+"_loaded",ut=n.browser.msie&&!n.support.opacity,yt=ut&&n.browser.version<7,at=s+"_IE6",k,o,tt,v,ii,ri,kt,ni,e,c,h,ht,st,dt,ti,it,et,ft,gt,g,nt,b,w,l,vt,f,i,p,rt,lt=!1,r,ot=s+"Element";r=n.fn[y]=n[y]=function(t,i){var r=this,u;return!r[0]&&r.selector?r:(t=t||{},i&&(t.onComplete=i),r[0]&&r.selector!==undefined||(r=n("<a/>"),t.open=!0),r.each(function(){n.data(this,y,n.extend({},n.data(this,y)||ui,t)),n(this).addClass(ot)}),u=t.open,n.isFunction(u)&&(u=u.call(r)),u&&fi(r[0]),r)},r.init=function(){c=n(t),o=u().attr({id:y,"class":ut?s+"IE":""}),k=u("Overlay",yt?"position:absolute":"").hide(),tt=u("Wrapper"),v=u("Content").append(h=u("LoadedContent","width:0; height:0; overflow:hidden"),st=u("LoadingOverlay").add(u("LoadingGraphic")),dt=u("Title"),ti=u("Current"),et=u("Next"),ft=u("Previous"),it=u("Slideshow").bind(hi,li),gt=u("Close")),tt.append(u().append(u("TopLeft"),ii=u("TopCenter"),u("TopRight")),u(!1,"clear:left").append(ri=u("MiddleLeft"),v,kt=u("MiddleRight")),u(!1,"clear:left").append(u("BottomLeft"),ni=u("BottomCenter"),u("BottomRight"))).children().children().css({float:"left"}),ht=u(!1,"position:absolute; width:9999px; visibility:hidden; display:none"),n("body").prepend(k,o.append(tt,ht)),v.children().hover(function(){n(this).addClass("hover")},function(){n(this).removeClass("hover")}).addClass("hover"),g=ii.height()+ni.height()+v.outerHeight(!0)-v.height(),nt=ri.width()+kt.width()+v.outerWidth(!0)-v.width(),b=h.outerHeight(!0),w=h.outerWidth(!0),o.css({"padding-bottom":g,"padding-right":nt}).hide(),et.click(r.next),ft.click(r.prev),gt.click(r.close),v.children().removeClass("hover"),n("."+ot).live("click",function(n){n.button!==0&&typeof n.button!="undefined"||n.ctrlKey||n.shiftKey||n.altKey||(n.preventDefault(),fi(this))}),k.click(function(){i.overlayClose&&r.close()}),n(document).bind("keydown",function(n){p&&i.escKey&&n.keyCode===27&&(n.preventDefault(),r.close()),p&&i.arrowKey&&!rt&&e[1]&&(n.keyCode===37&&(f||i.loop)?(n.preventDefault(),ft.click()):n.keyCode===39&&(f<e.length-1||i.loop)&&(n.preventDefault(),et.click()))})},r.remove=function(){o.add(k).remove(),n("."+ot).die("click").removeData(y).removeClass(ot)},r.position=function(n,t){function r(n){ii[0].style.width=ni[0].style.width=v[0].style.width=n.style.width,st[0].style.height=st[1].style.height=v[0].style.height=ri[0].style.height=kt[0].style.height=n.style.height}var u,f=Math.max(document.documentElement.clientHeight-i.h-b-g,0)/2+c.scrollTop(),e=Math.max(c.width()-i.w-w-nt,0)/2+c.scrollLeft();u=o.width()===i.w+w&&o.height()===i.h+b?0:n,tt[0].style.width=tt[0].style.height="9999px",o.dequeue().animate({width:i.w+w,height:i.h+b,top:f,left:e},{duration:u,complete:function(){r(this),rt=!1,tt[0].style.width=i.w+w+nt+"px",tt[0].style.height=i.h+b+g+"px",t&&t()},step:function(){r(this)}})},r.resize=function(n){if(p){n=n||{},n.width&&(i.w=a(n.width,"x")-w-nt),n.innerWidth&&(i.w=a(n.innerWidth,"x")),h.css({width:i.w}),n.height&&(i.h=a(n.height,"y")-b-g),n.innerHeight&&(i.h=a(n.innerHeight,"y"));if(!n.innerHeight&&!n.height){var t=h.wrapInner("<div style='overflow:auto'></div>").children();i.h=t.height(),t.replaceWith(t.children())}h.css({height:i.h}),r.position(i.transition==="none"?0:i.speed)}},r.prep=function(t){function k(){return i.w=i.w||h.width(),i.w=i.mw&&i.mw<i.w?i.mw:i.w}function b(){return i.h=i.h||h.height(),i.h=i.mh&&i.mh<i.h?i.mh:i.h}function a(t){var b,u,k,a,v=e.length,g=i.loop;r.position(t,function(){function t(){ut&&(o[0].style.filter=!1)}if(!p)return;ut&&w&&h.fadeIn(100),h.show(),d(si),dt.show().html(i.title),v>1&&(typeof i.current=="string"&&ti.html(i.current.replace(/\{current\}/,f+1).replace(/\{total\}/,v)).show(),et[g||f<v-1?"show":"hide"]().html(i.next),ft[g||f?"show":"hide"]().html(i.previous),b=f?e[f-1]:e[v-1],k=f<v-1?e[f+1]:e[0],i.slideshow&&it.show(),i.preloading&&(a=n.data(k,y).href||k.href,u=n.data(b,y).href||b.href,a=n.isFunction(a)?a.call(k):a,u=n.isFunction(u)?u.call(b):u,pt(a)&&(n("<img/>")[0].src=a),pt(u)&&(n("<img/>")[0].src=u))),st.hide(),i.transition==="fade"?o.fadeTo(l,1,function(){t()}):t(),c.bind("resize."+s,function(){r.position(0)}),d(bt,i.onComplete)})}if(!p)return;var w,l=i.transition==="none"?0:i.speed;c.unbind("resize."+s),h.remove(),h=u("LoadedContent").html(t),h.hide().appendTo(ht.show()).css({width:k(),overflow:i.scrolling?"auto":"hidden"}).css({height:b()}).prependTo(v),ht.hide(),n("#"+s+"Photo").css({cssFloat:"none",marginLeft:"auto",marginRight:"auto"});if(yt)n("select").not(o.find("select")).filter(function(){return this.style.visibility!=="hidden"}).css({visibility:"hidden"}).one(ci,function(){this.style.visibility="inherit"});i.transition==="fade"?o.fadeTo(l,0,function(){a(0)}):a(l)},r.load=function(t){var v,c,k,p=r.prep;rt=!0,l=e[f],t||(i=ei(n.extend({},n.data(l,y)))),d(ct),d(wt,i.onLoad),i.h=i.height?a(i.height,"y")-b-g:i.innerHeight&&a(i.innerHeight,"y"),i.w=i.width?a(i.width,"x")-w-nt:i.innerWidth&&a(i.innerWidth,"x"),i.mw=i.w,i.mh=i.h,i.maxWidth&&(i.mw=a(i.maxWidth,"x")-w-nt,i.mw=i.w&&i.w<i.mw?i.w:i.mw),i.maxHeight&&(i.mh=a(i.maxHeight,"y")-b-g,i.mh=i.h&&i.h<i.mh?i.h:i.mh),v=i.href,st.show();if(i.inline){u().hide().insertBefore(n(v)[0]).one(ct,function(){n(this).replaceWith(h.children())});p(n(v))}else if(i.iframe){o.one(si,function(){var t=n("<iframe name='"+ +new Date+"' frameborder=0"+(i.scrolling?"":" scrolling='no'")+(ut?" allowtransparency='true'":"")+" style='width:100%; height:100%; border:0; display:block;'/>");t[0].src=i.href;t.appendTo(h).one(ct,function(){t[0].src="about:blank"})});p(" ")}else i.html?p(i.html):pt(v)?(c=new Image,c.onload=function(){var t;c.onload=null,c.id=s+"Photo",n(c).css({border:"none",display:"block",cssFloat:"left"}),i.scalePhotos&&(k=function(){c.height-=c.height*t,c.width-=c.width*t},i.mw&&c.width>i.mw&&(t=(c.width-i.mw)/c.width,k()),i.mh&&c.height>i.mh&&(t=(c.height-i.mh)/c.height,k())),i.h&&(c.style.marginTop=Math.max(i.h-c.height,0)/2+"px"),e[1]&&(f<e.length-1||i.loop)&&n(c).css({cursor:"pointer"}).click(r.next),ut&&(c.style.msInterpolationMode="bicubic"),setTimeout(function(){p(c)},1)},setTimeout(function(){c.src=v},1)):v&&ht.load(v,function(t,i,r){p(i==="error"?"Request unsuccessful: "+r.statusText:n(this).children())})},r.next=function(){rt||(f=f<e.length-1?f+1:0,r.load())},r.prev=function(){rt||(f=f?f-1:e.length-1,r.load())},r.close=function(){p&&!lt&&(lt=!0,p=!1,d(ci,i.onCleanup),c.unbind("."+s+" ."+at),k.fadeTo("fast",0),o.stop().fadeTo("fast",0,function(){d(ct),h.remove(),o.add(k).css({opacity:1,cursor:"auto"}).hide();try{vt.focus()}catch(n){}setTimeout(function(){lt=!1,d(oi,i.onClosed)},1)}))},r.element=function(){return n(l)},r.settings=ui,n(r.init)})(jQuery,this),function(n){n.fn.tipsy=function(t){return t=n.extend({},n.fn.tipsy.defaults,t),this.each(function(){var i=n.fn.tipsy.elementOptions(this,t);n(this).hover(function(){var t,e,r;n.data(this,"cancel.tipsy",!0),t=n.data(this,"active.tipsy"),t||(t=n('<div class="tipsy"><div class="tipsy-inner"/></div>'),t.css({position:"absolute",zIndex:1e5}),n.data(this,"active.tipsy",t)),(n(this).attr("title")||typeof n(this).attr("original-title")!="string")&&n(this).attr("original-title",n(this).attr("title")||"").removeAttr("title"),typeof i.title=="string"?e=n(this).attr(i.title=="title"?"original-title":i.title):typeof i.title=="function"&&(e=i.title.call(this)),t.find(".tipsy-inner")[i.html?"html":"text"](e||i.fallback),r=n.extend({},n(this).offset(),{width:this.offsetWidth,height:this.offsetHeight}),t.get(0).className="tipsy",t.remove().css({top:0,left:0,visibility:"hidden",display:"block"}).appendTo(document.body);var f=t[0].offsetWidth,u=t[0].offsetHeight,o=typeof i.gravity=="function"?i.gravity.call(this):i.gravity;switch(o.charAt(0)){case"n":t.css({top:r.top+r.height,left:r.left+r.width/2-f/2}).addClass("tipsy-north");break;case"s":t.css({top:r.top-u,left:r.left+r.width/2-f/2}).addClass("tipsy-south");break;case"e":t.css({top:r.top+r.height/2-u/2,left:r.left-f}).addClass("tipsy-east");break;case"w":t.css({top:r.top+r.height/2-u/2,left:r.left+r.width}).addClass("tipsy-west")}i.fade?t.css({opacity:0,display:"block",visibility:"visible"}).animate({opacity:.8}):t.css({visibility:"visible"})},function(){n.data(this,"cancel.tipsy",!1);var t=this;setTimeout(function(){if(n.data(this,"cancel.tipsy"))return;var r=n.data(t,"active.tipsy");i.fade?r.stop().fadeOut(function(){n(this).remove()}):r.remove()},100)})})},n.fn.tipsy.elementOptions=function(t,i){return n.metadata?n.extend({},i,n(t).metadata()):i},n.fn.tipsy.defaults={fade:!1,fallback:"",gravity:"n",html:!1,title:"title"},n.fn.tipsy.autoNS=function(){return n(this).offset().top>n(document).scrollTop()+n(window).height()/2?"s":"n"},n.fn.tipsy.autoWE=function(){return n(this).offset().left>n(document).scrollLeft()+n(window).width()/2?"e":"w"}}(jQuery)