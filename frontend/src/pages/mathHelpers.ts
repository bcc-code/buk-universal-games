export function floatToInt(num:number):number{
  return Math.round(num)
}

export function clamp<T extends number>(min:T, value:T, max:T):T{
  if(min >=max ) throw Error("min is more or equal to max");

  return Math.max(min,Math.min(value,max)) as T;
}

export function lerp(
  inputMin: number,
  inputMax: number,
  outputMin: number,
  outputMax: number,
  value: number,
  handleOutside :"throw"|"clamp"|"extend"= "throw"
): number {
  const isOutside = value < inputMin || value > inputMax;
  if (isOutside && handleOutside === "throw") 
    throw new Error(`Value ${value} is outside the input range ${inputMin}-${inputMax}`)
  
  const lerpedValue = outputMin + ((value - inputMin) * (outputMax - outputMin)) / (inputMax - inputMin);

  if(isOutside && handleOutside ==="clamp") return clamp(Math.min(outputMin,outputMax), lerpedValue, Math.max(outputMin,outputMax));

  return lerpedValue;
}
