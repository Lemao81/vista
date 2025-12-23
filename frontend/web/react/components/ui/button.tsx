import { ButtonHTMLAttributes, DetailedHTMLProps, HTMLProps, ReactNode } from 'react';
import { cn } from '@/utils/cn';

export type ButtonProps = {
  text: ReactNode | string;
  type?: ButtonHTMLAttributes<HTMLButtonElement>['type'];
} & DetailedHTMLProps<HTMLProps<HTMLButtonElement>, HTMLButtonElement>;

export default function Button({ text, type, className, ...props }: ButtonProps) {
  return (
    <button
      className={cn(
        'rounded-lg px-4 py-1.5 shadow-md transition-all active:scale-95 cursor-pointer text-white',
        className
      )}
      type={type || 'button'}
      {...props}
    >
      {text}
    </button>
  );
}
