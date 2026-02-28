import type { DetailedHTMLProps, HTMLProps } from 'react';
import { cn } from '@/utils/cn';

export type InputProps = {
  label: string;
  placeholder?: string;
  isPassword?: boolean;
  error?: string;
} & DetailedHTMLProps<HTMLProps<HTMLInputElement>, HTMLInputElement>;

export default function Input({ label, placeholder, isPassword, error, ...props }: InputProps) {
  return (
    <div className="grid gap-[0.25lh] form-group">
      <label className="font-semibold" htmlFor="input">
        {label}
      </label>
      <input
        className={cn(
          'rounded-lg bg-white px-2 py-1 shadow-md ring-1 ring-neutral-400 focus:outline-0',
          error && 'ring-red-400'
        )}
        placeholder={placeholder}
        type={isPassword ? 'password' : 'text'}
        {...props}
      />
      {error ? (
        <span className="text-sm text-red-600">{error}</span>
      ) : (
        <span className="text-sm invisible">.</span>
      )}
    </div>
  );
}
