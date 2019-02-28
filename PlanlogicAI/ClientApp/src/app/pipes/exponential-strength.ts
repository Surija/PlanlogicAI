import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'exponentialStrength' })
export class ExponentialStrengthPipe implements PipeTransform {
    transform(value: number, exponent: string): number {
        let exp = parseFloat(exponent);
        return Math.pow(value, isNaN(exp) ? 1 : exp);
    }
}

@Pipe({
    name: 'positive'
})
export class MinusSignToParens implements PipeTransform {

    transform(value: any, args?: any): any {
        return Math.abs(value);
    }
}
@Pipe({
    name: 'unutilized'
})
export class UnutilizedProductPipe implements PipeTransform {

    transform(items: Array<any>, sel: number): Array<any> {       
        var values = items.filter(item => item.unutilizedValue != 0);
        alert(JSON.stringify(values));
        return values;
    }
}