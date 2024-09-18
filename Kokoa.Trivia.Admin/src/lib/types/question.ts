import type { Option } from './option';

export interface Question {
	id: number;
	title: string;
	correct_option: Option;
	options: Option[];
}
