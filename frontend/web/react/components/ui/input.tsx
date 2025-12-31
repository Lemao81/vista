import { DetailedHTMLProps, HTMLProps } from 'react';
import { cn } from '@/utils/cn';

export type InputProps = {
  label: string;
  placeholder?: string;
  isPassword?: boolean;
  error?: string;
} & DetailedHTMLProps<HTMLProps<HTMLInputElement>, HTMLInputElement>;

export default function Input({ label, placeholder, isPassword, error, ...props }: InputProps) {
  return (
    <div className="flex flex-col w-full">
      <label className="font-semibold">{label}</label>
      <input
        className={cn(
          'rounded-lg bg-white px-2 py-1 shadow-md ring-1 ring-neutral-400 focus:outline-0 mt-2',
          error && 'ring-red-400'
        )}
        type={isPassword ? 'password' : 'text'}
        placeholder={placeholder}
        {...props}
      />
      {error ? (
        <span className="text-sm text-red-600 mt-1">{error}</span>
      ) : (
        <span className="invisible">.</span>
      )}
    </div>
  );
}
