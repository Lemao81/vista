import React, { type PropsWithChildren } from 'react';
import ModalCanvas from '@/components/shared/modal-canvas';
import Overlay from '@/components/shared/overlay';

export type ModalProps = {
  show: boolean;
  onDismiss?: () => void;
} & PropsWithChildren;

export default function Modal({ children, show, onDismiss }: ModalProps) {
  return (
    <div style={{ display: show ? 'block' : 'none' }}>
      <Overlay onDismiss={() => onDismiss?.()}>
        <ModalCanvas>{children}</ModalCanvas>
      </Overlay>
    </div>
  );
}
