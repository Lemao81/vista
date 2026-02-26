import type { DetailedHTMLProps, HTMLProps } from 'react';

export type CheckboxProps = {
  label: string;
  error?: string;
} & DetailedHTMLProps<HTMLProps<HTMLInputElement>, HTMLInputElement>;

export default function Checkbox({ label, error, ...props }: CheckboxProps) {
  return (
    <div className="flex flex-col w-full">
      <div className="flex gap-[1ch] bg-transparent px-2 py-1">
        <input type="checkbox" {...props} />
        <label className="font-semibold text-[15px]">{label}</label>
      </div>
      {error ? (
        <span className="text-sm text-red-600">{error}</span>
      ) : (
        <span className="text-sm invisible">.</span>
      )}
    </div>
  );
}
