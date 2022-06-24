handleVideo();
window.addEventListener('resize',handleVideo)
window.onload = function(){
    LoadVideo();
    loginCheck();
    toonAantalProducten();
}
function handleVideo(){
    let width = window.innerWidth;
    const breakpoint = 1650
    let negativemargin;
    if(width<breakpoint){
        negativemargin=width-breakpoint;
        console.log(negativemargin);
        document.getElementById("windmolensvid").style.marginLeft=`${negativemargin}px`;
    }
    else if(width>breakpoint){
        document.getElementById("windmolensvid").style.marginLeft="0";
    }
}

function LoadVideo(){
    let videonumber = Math.floor(Math.random()*7)+1;
    let videosource = document.createElement("source");
    console.log(`LandingPageVideo (${videonumber}).mp4`);
    videosource.src = `../assets/videos/LandingPageVideo (${videonumber}).mp4`;
    videosource.type = "video/mp4";
    document.getElementById("windmolensvid").appendChild(videosource);
}