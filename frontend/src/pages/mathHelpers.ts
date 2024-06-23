// 🧹 we should be able to take decimals on the backend, and save it. 
export function floatToInt(num:number):number{
  return Math.round(num)
}

export function clamp(min:number, value:number, max:number):number{
  return Math.max(min,Math.min(value,max)) ;
}

export function lerp(
  inputMin: number,
  inputMax: number,
  outputMin: number,
  outputMax: number,
  value: number
): number {
  if (value < inputMin || value > inputMax) {
    throw new Error(`Value ${value} is outside the input range ${inputMin}-${inputMax}`);
  }
  return outputMin + ((value - inputMin) * (outputMax - outputMin)) / (inputMax - inputMin);
}
