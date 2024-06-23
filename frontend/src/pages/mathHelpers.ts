// 🧹 we should be able to take decimals on the backend, and save it. 
export function floatToInt(num:number):number{
  return Math.round(num)
}

export function clamp(min:number, value:number, max:number):number{
  return Math.max(min,Math.min(value,max)) ;
}
