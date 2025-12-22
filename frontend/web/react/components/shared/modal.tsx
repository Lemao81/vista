import Overlay from '@/components/shared/overlay';
import ModalCanvas from '@/components/shared/modal-canvas';
import React, { PropsWithChildren } from 'react';

export type ModalProps = {
  show: boolean;
} & PropsWithChildren;

export default function Modal({ show, children }: ModalProps) {
  return (
    <div style={{ display: show ? 'block' : 'none' }}>
      <Overlay>
        <ModalCanvas>{children}</ModalCanvas>
      </Overlay>
    </div>
  );
}
