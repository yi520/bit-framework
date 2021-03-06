﻿module Bit.Implementations {

    export class DefaultProvider {

        public static buildProvider(): void {

            Provider.getFormattedDateDelegate = (date: Date, culture: string) => {
                if (culture == "FaIr")
                    return new window["persianDate"](date).format("YYYY/MM/DD") as string;
                else
                    return kendo.toString(date, "yyyy/dd/MM");
            };

            Provider.getFormattedDateTimeDelegate = (date: Date, culture: string) => {

                if (culture == "FaIr")
                    return new window["persianDate"](date).format("DD MMMM YYYY, hh:mm a") as string;
                else
                    return kendo.toString(date, "yyyy/dd/MM, hh:mm tt");

            };

            Provider.parseDateDelegate = (date: any) => {
                return kendo.parseDate(date);
            };

            Provider.loggerProvider = () => DependencyManager.getCurrent().resolveObject<Contracts.ILogger>("Logger");
        }
    }
}