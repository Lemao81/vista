import { cva, type VariantProps } from 'class-variance-authority';
import type { ButtonHTMLAttributes, DetailedHTMLProps, HTMLProps, ReactNode } from 'react';
import { cn } from '@/utils/cn';

const buttonVariants = cva(
  'shadow-md transition-all active:scale-95 cursor-pointer text-white disabled:bg-gray-400 disabled:pointer-events-none',
  {
    variants: {
      intent: {
        brand: 'bg-brand hover:bg-(--brand-bright) active:bg-(--brand-dim)',
        cancel: 'border surface3 hover active',
      },
      extent: {
        n: 'rounded-lg px-4 py-1.5',
        lg: 'rounded-md px-5 py-2',
      },
      pulse: {
        true: 'hover:scale-105',
        false: null,
      },
    },
    defaultVariants: {
      intent: 'brand',
      extent: 'n',
      pulse: false,
    },
  }
);

export type ButtonProps = {
  text: ReactNode | string;
  type?: ButtonHTMLAttributes<HTMLButtonElement>['type'];
} & DetailedHTMLProps<HTMLProps<HTMLButtonElement>, HTMLButtonElement> &
  VariantProps<typeof buttonVariants>;

export default function Button({
  text,
  type,
  className,
  intent,
  extent,
  pulse,
  ...props
}: ButtonProps) {
  return (
    <button
      className={cn(buttonVariants({ intent, extent, pulse }), className)}
      type={type || 'button'}
      {...props}
    >
      {text}
    </button>
  );
}
