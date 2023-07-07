<template>
  <div class="circle center">
      <div class="count">{{timeRemaining}}</div>
      <div class="l-half"></div>
      <div class="r-half"></div>
  </div>
    </template>
  
  <!-- https://jsfiddle.net/zshLfgm9/11/ -->
  
    <script>
    export default {
      props: {
        seconds: {
          type: Number,
          required: true,
        },
        size: {
          type: Number,
          default: 100,
        },
        lineThickness: {
          type: Number,
          default: 12,
        },
        color: {
          type: String,
          default: '#f00',
        },
      },
      emits: ['timer-finished'],
      data() {
        return {
          secondsPlusOne: this.seconds + 1,
          fontSize: ((this.size - (this.lineThickness * 2)) / 2) + 'px',
          halfSize: (this.size / 2) + 'px',
          lineThicknessPx: this.lineThickness + 'px',
          sizePx: this.size + 'px',
          secondsCss: this.seconds + 's',
          timeRemaining: this.seconds
        };
      },
      mounted() {
        this.startTimer();
      },
      methods: {
        startTimer() {
          const interval = setInterval(() => {
            this.timeRemaining--;
            if (this.timeRemaining <= 0) {
              clearInterval(interval);
              this.$emit("timer-finished");
            }
          }, 1000);
        },
      },
    };
    </script>
    
    <style>
  
  .center {
      position: absolute;
      top: 50%;
      left: 50%;
      margin-top: -v-bind(halfSize);
      margin-left: -v-bind(halfSize);
  }
  
  /* -- CIRCLE -- */
  
  .circle {
      width: v-bind(sizePx);
      height: v-bind(sizePx);
      position: relative;
      border-radius: 999px;
      box-shadow: inset 0 0 0 v-bind(lineThicknessPx) rgba(255,255,255,0.5);
  }
  
  .l-half, .r-half {
      float: left;
      width: 50%;
      height: 100%;
      overflow: hidden;          
  }

  .l-half:before, .r-half:before {
          content: "";
          display: block;
          width: 100%;
          height: 100%;
          box-sizing: border-box;
          border: v-bind(lineThicknessPx) solid v-bind(color);
          animation-duration: v-bind(secondsCss);
          animation-iteration-count: 1;
          animation-timing-function: linear;
          animation-fill-mode: forwards;
  }
  
  .l-half:before {
      border-right: none;
      border-top-left-radius: 999px;
      border-bottom-left-radius: 999px;
      transform-origin: center right;
      animation-name: l-rotate;
  }
      
  .r-half:before {
      border-left: none;
      border-top-right-radius: 999px;
      border-bottom-right-radius: 999px;
      transform-origin: center left;
      animation-name: r-rotate;
  }
  
  /* -- TIMER -- */
  
  .count {
      position: absolute;
      width: 100%;
      line-height: v-bind(sizePx);
      text-align: center;
      font-weight: 800;
      font-size: v-bind(fontSize);
      font-family: Helvetica;
      color: v-bind(color);
      z-index: 2;
      animation: fadeout .5s v-bind(secondsPlusOne) 1 linear;
      animation-fill-mode: forwards;
  }

  @keyframes l-rotate {
        0% { transform: rotate(0deg); }
        50% { transform: rotate(-180deg); }
        100% { transform: rotate(-180deg); }
    }
    
    @keyframes r-rotate {
        0% { transform: rotate(0deg); }
        50% { transform: rotate(0deg); }
        100% { transform: rotate(-180deg); }
    }
    
    @keyframes fadeout {
        0% { opacity: 1; }
        100% { opacity: 0.5; }
    }
    </style>