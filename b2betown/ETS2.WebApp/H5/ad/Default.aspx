<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ETS2.WebApp.H5.ad.Default" %>

<!doctype html>

<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1, user-scalable=no">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <link rel="shortcut icon" href="/favicon.ico">

    <title><%=Title %></title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="js/jquery.fullPage.css">
    <link rel="stylesheet" href="js/mobile.css">

    <style>
      .page-day {
        background-position: center;
        background-repeat: no-repeat;
        background-size: cover;
      }

      .m-st {
        display: none;
        position: fixed;
        right: 2%;
        top: 2%;
        z-index: 1;
        height: 45px;
      }

      .m-ft {
        position: fixed;
        left: 0;
        right: 0;
        bottom: 12px;
        z-index: 1;
      }

      .m-down {
        position: fixed;
        left: 0;
        right: 0;
        bottom: 72px;
        z-index: 1;
        height: 10px;
        margin: auto;
      }

      .m-back {
        height: 50px;
      }

      .m-ft .btn-block {
        width: 80%;
        background: #aaa251;
        border: #aaa251;
        color: #fff;
        margin-top: 2px;
        border-radius: 25px;
      }

      .mp-background {
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        z-index: 1;
      }

      .mp-back {
        text-align: center;
        position: absolute;
        z-index: 2; 
        left: 0;
        right: 0;
        margin: auto;
      }

      .mp-back1 {
        width: 80%;
        top: 4%;
      }
      .mp-back2 {
        width: 25%;
        top: 50%;
      }
      .mp-back-rec {
        top: 60%;
      }

      .mp-back-rec > img {
        width: 80%;
      }

      .modal.m-modal .modal-container {
        text-align: center;
        background: transparent;
      }
      .modal.m-modal.modal-qr .modal-container > img,
      .modal.m-modal.modal-re .modal-container > img {
        width: 67%;
      }

      .icon.thumb {
        background: url(images/m-thumb.png) no-repeat center / 1.4rem 1.4rem;
        display: inline-block;
        width: 1.4rem;
        height: 1.4rem;
        font-size: 1.4rem;
        margin-right: .5rem;
      }

      .vote-btn.active > i.thumb {
        -webkit-animation: scale-up .75s ease;
        animation: scale-up .75s ease;
      }

      /*scale up animation*/
      @-webkit-keyframes scale-up {
        0% {
          -webkit-transform: translateY(0);
          transform: translateY(0);
          opacity: 1;
        }

        100% {
          -webkit-transform: translateY(-100%);
          transform: translateY(-100%);
          opacity: 0;
        }
      }

      @keyframes scale-up {
        0% {
          -webkit-transform: translateY(0);
          transform: translateY(0);
          opacity: 1;
        }

        100% {
          -webkit-transform: translateY(-100%);
          transform: translateY(-100%);
          opacity: 0;
        }
      }
      
      
       #audio_btn {
    position: absolute;
    right: 20px;
    top: 20px;
    z-index: 200;
    display: none;
    width: 30px;
    height: 30px;
    background-repeat: no-repeat;
	}
	.off {
		background-image: url(/images/normalmusic.svg);
		background-size: contain;
		background-repeat: no-repeat;
	}
	.rotate {
    -webkit-animation: rotating 1.2s linear infinite;
    -moz-animation: rotating 1.2s linear infinite;
    -o-animation: rotating 1.2s linear infinite;
    animation: rotating 1.2s linear infinite;
	}

	
	@-webkit-keyframes rotating {
		from {
			-webkit-transform:rotate(0deg)
		}
		to {
			-webkit-transform:rotate(360deg)
		}
	}
	@keyframes rotating {
		from {
			/*! transform:rotate(0deg) */
		}
		to {
			/*! transform:rotate(360deg) */
		}
	}
	@-moz-keyframes rotating {
		from {
			-moz-transform:rotate(0deg);
		}
		to {
			 -moz-transform:rotate(360deg); 
		}
	}
    </style>
    <script type="text/javascript">

        $(function () {
            $("#audio_btn").click(function () {
                var audioEle = $("#media")[0];
                if ($('#audio_btn').is('.rotate')) {
                    audioEle.pause(); //暂停
                    $('#audio_btn').removeClass('rotate')
                } else {
                    audioEle.play(); //播放
                    $('#audio_btn').addClass('rotate')
                }
            })

        })

  </script>
  </head>

  <body>

  <img src="images/m-st.png" class="m-st" alt="">
  <img src="images/down.png" class="m-down" alt="">
  <div class="m-ft">
    <div class="flex">
      <div class="text-center col-3" ><img src="images/m-back.png" class="m-back js-mp-back" style=" display:none;" alt=""></div>
      <div class="text-center col-9"><button class="btn btn-block btn-lg vote-btn js-vote-btn"><span class="vote-count" >立即预订</span></button></div>
    </div>
  </div>
  <%if (Musicid != 0)
    { %>
      <div style="display: block;" id="audio_btn" class="off video_exist rotate">
            <audio loop="" src="<%=Musicscr %>" id="media" autoplay="" preload=""></audio>
  </div>
 
  <%} %>
  <div class="pages">
  </div>

  <div class="modal m-modal modal-qr">
    <div class="modal-overlay"></div>
    <div class="modal-container">
      <!-- <button class="btn btn-clear float-right"></button> -->
      <img class="image-qr" src="" alt="">
    </div>
  </div>

  <div class="modal m-modal modal-re">
    <div class="modal-overlay"></div>
    <div class="modal-container">
      <!-- <button class="btn btn-clear float-right"></button> -->
      <img class="image-re" src="images/vote-re.png" alt="">
    </div>
  </div>


  <script type="text/template" id="template-fullpage">


  <div class="section page-day" style="background:url(${Imageurl}) no-repeat 50% 100% / cover;">

  </div>

 
  </script>


  <script src="js/jquery.js"></script>
  <script src="js/jquery.fullPage.js"></script>
  <script src="js/fastclick.js"></script>
  <script src="js/underscore.js"></script>
  <script src="//res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
  <script src="js/mobile.js"></script>
  <script src="/Scripts/jquery.pager.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.tmpl.min.js" type="text/javascript"></script>
  <script>

      $(document).ready(docReady);

      function docReady() {
          var id = _query['id'];
          var rp = _.sample(_tools._ids);

          if (!id) {
              //return show404();
          }

          FastClick.attach(document.body);


          $('body').attr('data-cid', id);

          $.ajax({
              type: "post",
              url: "/JsonFactory/WeiXinHandler.ashx?oper=Getwxadimagespagelist",
              data: { comid: $("#hid_comid").val(), pageindex: 1, pagesize: 100, adid: $("#hid_id").val() },
              async: false,
              success: function (data) {
                  data = eval("(" + data + ")");

                  if (data.type == 1) {
                      return show404();

                  }
                  if (data.type == 100) {
                      if (data.totalCount == 0) {
                          return show404();
                      } else {
                          $("#template-fullpage").tmpl(data.msg).appendTo(".pages");
                      }

                  }
              }
          })

          $(".js-vote-btn").click(function () {
              window.location.href = '<%=Link %>';
          })

          $('.pages').fullpage({
              continuousVertical: true
          });

          $('body').on('click', '.modal-qr', hide);
          $('body').on('click', '.modal-re', back);

          $('.js-mp-back').on('click', back);
      }

      function vote(e) {
          _tools.vote(e);
      }

      function back(e) {
          e.preventDefault();
          //window.location.href = './m.html?from=mp';
      }

      function hide() {
          $('.m-modal').removeClass('active');
          $('body').removeClass('m-modal-show');
      }

      function show404() {
          $('body').html('<h2 style="font-weight:100">404 Not Found :(</h1>');
      }

  </script>
  <input type="hidden" id="hid_id" value="<%=id %>" />
    <input type="hidden" id="hid_comid" value="<%=comid %>" />
  </body>
</html>