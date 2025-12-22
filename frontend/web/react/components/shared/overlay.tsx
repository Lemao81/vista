import React, { PropsWithChildren } from 'react';

export type OverLayProps = {
  onDismiss?: () => void;
} & PropsWithChildren;

export default function Overlay({ children, onDismiss }: OverLayProps) {
  return (
    <>
      <div className="fixed inset-0 z-1000 bg-black opacity-60"></div>
      <div
        className="fixed inset-0 z-1001 flex items-center justify-center"
        onClick={() => onDismiss?.()}
      >
        {children}
      </div>
    </>
  );
}
