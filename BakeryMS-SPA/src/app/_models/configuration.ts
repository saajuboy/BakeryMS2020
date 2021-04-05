export interface Configuration {
    id: number;
    description: string;
    value: string;
    userId?: number;

}

export interface ConfigurationList {
    configurations: Configuration[];
}
