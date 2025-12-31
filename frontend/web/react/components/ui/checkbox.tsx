import { DetailedHTMLProps, HTMLProps } from 'react';

export type CheckboxProps = {
  label: string;
  error?: string;
} & DetailedHTMLProps<HTMLProps<HTMLInputElement>, HTMLInputElement>;

export default function Checkbox({ label, error, ...props }: CheckboxProps) {
  return (
    <div className="flex flex-col w-full">
      <div className="flex gap-3 bg-transparent px-2 py-1">
        <input type="checkbox" {...props} />
        <label className="font-semibold text-[15px]">{label}</label>
      </div>
      {error ? (
        <span className="text-sm text-red-600 mt-1">{error}</span>
      ) : (
        <span className="invisible">.</span>
      )}
    </div>
  );
}
